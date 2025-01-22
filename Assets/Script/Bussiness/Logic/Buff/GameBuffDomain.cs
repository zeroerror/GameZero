using System;
using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameBuffDomain : GameBuffDomainApi
    {
        GameContext _context;
        GameBuffContext _buffContext => this._context.buffContext;

        public GameBuffDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick(float dt)
        {
            this._context.roleContext.repo.ForeachAllEntities(entity =>
            {
                if (!entity.IsAlive()) return;
                this.TickBuff(entity, dt);
            });
            this._context.actionContext.optionRepo.ForeachAllEntities(entity =>
            {
                if (!entity.IsAlive()) return;
                this.TickBuff(entity, dt);
            });
        }

        /// <summary> 角色实体和行为选项实体会持有buff组件 </summary>
        private GameBuffCom _GetBuffCom(GameEntityBase entity)
        {
            if (entity.TryGetLinkParent<GameRoleEntity>(out var role))
            {
                return role.buffCom;
            }
            if (entity.TryGetLinkParent<GameActionOptionEntity>(out var actionOption))
            {
                return actionOption.buffCom;
            }
            GameLogger.LogError("获取buff组件失败！未处理的buff目标类型：" + entity.idCom.entityType);
            return null;
        }

        public void TickBuff(GameEntityBase buffOwner, float dt)
        {
            var buffCom = this._GetBuffCom(buffOwner);
            buffCom.Foreach(buff =>
            {
                if (!buff.isValid) return;
                buff.Tick(dt);
                // buff层数选择器
                this._TickLayerMonitors(buff);
                // 行为条件 ps: 没有有效条件时默认为满足
                var isSatisfied_action = !buff.conditionSetEntity_action.IsValid() || buff.conditionSetEntity_action.CheckSatisfied();
                if (isSatisfied_action)
                {
                    buff.StartCD();
                    buff.model.actionIds?.Foreach((Action<int>)(actionId =>
                    {
                        buff.physicsCom.ClearCollided();
                        buff.attributeCom.CopyFrom(buff.idCom.parent.attributeCom);
                        buff.baseAttributeCom.CopyFrom(buff.idCom.parent.baseAttributeCom);
                        this._context.domainApi.actionApi.DoAction(actionId, buff, buff.GetActionParam());
                    }));
                }
                // 移除条件 ps: 没有有效条件时默认为不满足, 也就是默认
                var isSatisfied_remove = buff.conditionSetEntity_remove.IsValid() && buff.conditionSetEntity_remove.CheckSatisfied();
                if (isSatisfied_remove)
                {
                    this._context.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        this.TryDetachBuff(buffOwner, buff.model.typeId, 0);
                    });
                }
            });
        }

        /// <summary> 更新buff层数监视器对层数的影响 </summary>
        private void _TickLayerMonitors(GameBuffEntity buff)
        {
            var layerSelector = buff.model.layerSelector;
            var layerValueRefModel = buff.model.layerValueRefModel;
            var layerOffset = 0;
            if (layerSelector != null)
            {
                var selectedEntities = this._context.domainApi.entitySelectApi.SelectEntities(layerSelector, buff, false);
                var layer = selectedEntities?.Count ?? 0;
                if (layer == buff.layer) return;
                var targeterList = new List<GameActionTargeterArgs>();
                selectedEntities?.Foreach(selectedEntity =>
                {
                    var targeter = new GameActionTargeterArgs
                    {
                        targetEntity = selectedEntity,
                        targetPosition = selectedEntity.transformCom.position,
                        targetDirection = (selectedEntity.transformCom.position - buff.owner.transformCom.position).normalized
                    };
                    targeterList.Add(targeter);
                });

                buff.actionTargeterCom.SetTargeterList(targeterList);
                buff.actionTargeterCom.foreachType = GameForeachType.Sequential;
                layerOffset = layer - buff.layer;
            }
            else if (layerValueRefModel != null)
            {
                var actor = buff.idCom.parent;
                var target = buff.actionTargeterCom.getCurTargeter().targetEntity;
                var needRefTarget = layerValueRefModel.refType.IsTargetRef();
                if (needRefTarget && !target) return;
                var refValue = layerValueRefModel.refType.GetRefAttributeValue(actor, target, layerValueRefModel.value, layerValueRefModel.valueFormat);
                layerOffset = (int)refValue - buff.layer;
            }

            if (layerOffset > 0)
            {
                this._AttachLayer(buff, layerOffset);
            }
            else if (layerOffset < 0)
            {
                this._DetachLayer(buff, -layerOffset);
            }
        }

        /// <summary> 尝试挂载buff </summary>
        public bool TryAttachBuff(int typeId, GameEntityBase actor, GameEntityBase target, int layer, out int realAttachLayer)
        {
            // 如果actor是option, 则buff的行为者设置为target
            if (actor is GameActionOptionEntity)
            {
                actor = target;
            }

            realAttachLayer = 0;

            if (!target.TryGetLinkParent<GameRoleEntity>(out var targetRole))
            {
                GameLogger.LogWarning("目标不是角色, 暂不支持挂载Buff");
                return false;
            }

            // 目标已死亡
            if (!targetRole.IsAlive())
            {
                GameLogger.LogWarning("目标已死亡, 无法挂载Buff");
                return false;
            }

            var buffCom = targetRole.buffCom;
            if (buffCom.TryGet(typeId, out var existBuff))
            {
                realAttachLayer = this._AttachBuff(existBuff, targetRole, layer);
                return true;
            }

            var repo = this._buffContext.repo;
            if (!repo.TryFetch(typeId, out var newBuff))
            {
                newBuff = this._buffContext.factory.Load(typeId);
            }
            if (newBuff == null)
            {
                GameLogger.LogError("Buff创建失败, BuffID不存在：" + typeId);
                return false;
            }

            // 绑定父子关系
            newBuff.idCom.entityId = this._buffContext.idService.FetchId();
            newBuff.idCom.SetParent(actor);
            // 组件绑定
            newBuff.BindTransformCom(targetRole.transformCom);

            // 注入buff条件所需delegate
            this._InjectBuffConditionDelegate(newBuff, targetRole);

            buffCom.Add(newBuff);
            this._buffContext.repo.TryAdd(newBuff);
            realAttachLayer = this._AttachBuff(newBuff, targetRole, layer);
            return true;
        }

        private void _InjectBuffConditionDelegate(GameBuffEntity buff, GameRoleEntity targetRole)
        {
            buff.conditionSetEntity_remove.Inject(
                this._context.FindEntity,
                this._context.domainApi,
                this._ForeachActionRecord_Dmg,
                this._ForeachActionRecord_Heal,
                this._ForeachActionRecord_LaunchProjectile,
                this._ForeachActionRecord_KnockBack,
                this._ForeachActionRecord_AttributeModify,
                this._ForeachActionRecord_AttachBuff,
                this._ForeachActionRecord_SummonRoles,
                this._ForeachRoleStateRecord_Enter
            );
            buff.conditionSetEntity_action.Inject(
                this._context.FindEntity,
                this._context.domainApi,
                this._ForeachActionRecord_Dmg,
                this._ForeachActionRecord_Heal,
                this._ForeachActionRecord_LaunchProjectile,
                this._ForeachActionRecord_KnockBack,
                this._ForeachActionRecord_AttributeModify,
                this._ForeachActionRecord_AttachBuff,
                this._ForeachActionRecord_SummonRoles,
                this._ForeachRoleStateRecord_Enter
            );
        }
        private void _ForeachActionRecord_Dmg(in Action<GameActionRecord_Dmg> actionRecord)
        {
            this._context.actionContext.dmgRecordList.Foreach(actionRecord);
        }
        private void _ForeachActionRecord_Heal(in Action<GameActionRecord_Heal> actionRecord)
        {
            this._context.actionContext.healRecordList.Foreach(actionRecord);
        }
        private void _ForeachActionRecord_LaunchProjectile(in Action<GameActionRecord_LaunchProjectile> actionRecord)
        {
            this._context.actionContext.launchProjectileRecordList.Foreach(actionRecord);
        }
        private void _ForeachActionRecord_KnockBack(in Action<GameActionRecord_KnockBack> actionRecord)
        {
            this._context.actionContext.knockBackRecordList.Foreach(actionRecord);
        }
        private void _ForeachActionRecord_AttributeModify(in Action<GameActionRecord_AttributeModify> actionRecord)
        {
            this._context.actionContext.attributeModifyRecordList.Foreach(actionRecord);
        }
        private void _ForeachActionRecord_AttachBuff(in Action<GameActionRecord_AttachBuff> actionRecord)
        {
            this._context.actionContext.attachBuffRecordList.Foreach(actionRecord);
        }
        private void _ForeachActionRecord_SummonRoles(in Action<GameActionRecord_SummonRoles> actionRecord)
        {
            this._context.actionContext.summonRolesRecordList.Foreach(actionRecord);
        }
        private void _ForeachRoleStateRecord_Enter(in Action<GameRoleStateRecord> stateRecord)
        {
            this._context.roleContext.roleStateRecords.Foreach(stateRecord);
        }

        /// <summary> 执行挂载buff </summary>
        private int _AttachBuff(GameBuffEntity buff, GameRoleEntity targetRole, int layer)
        {
            // 设置目标
            buff.owner = targetRole;

            // 设置行为目标
            buff.actionTargeterCom.SetTargeter(new GameActionTargeterArgs
            {
                targetEntity = targetRole,
                targetPosition = targetRole.transformCom.position,
                targetDirection = targetRole.forward
            });

            // 挂载层数
            int realLayer;
            if (buff.model.HasLayerMonitor())
            {
                this._TickLayerMonitors(buff);
                realLayer = buff.layer;
            }
            else
            {
                realLayer = this._AttachLayer(buff, layer);
            }

            // 提交RC事件
            this._context.SubmitRC(GameBuffRCCollection.RC_GAME_BUFF_ATTACH, new GameBuffRCArgs_Attach
            {
                buffIdArgs = buff.idCom.ToArgs(),
                targetIdArgs = targetRole.idCom.ToArgs(),
                layer = realLayer
            });

            return realLayer;
        }

        /// <summary> 执行挂载层数 </summary>
        private int _AttachLayer(GameBuffEntity buff, int layer)
        {
            var buffModel = buff.model;
            var beforeLayer = buff.layer;

            // 时间刷新
            var refreshFlag = buffModel.refreshFlag;
            if (refreshFlag.HasFlag(GameBuffRefreshFlag.RefreshTime))
            {
                buff.conditionSetEntity_remove.RefreshTime();
            }

            // 时间叠加
            if (refreshFlag.HasFlag(GameBuffRefreshFlag.StackTime))
            {
                buff.conditionSetEntity_remove.StackTime();
            }

            // 层数叠加
            if (refreshFlag.HasFlag(GameBuffRefreshFlag.StackLayer) || beforeLayer == 0)
            {
                var afterLayer = beforeLayer + layer;
                var maxLayer = buffModel.maxLayer == 0 ? int.MaxValue : buffModel.maxLayer;// 0表示最大层数不限制
                afterLayer = GameMath.Min(afterLayer, maxLayer);
                buff.layer = afterLayer;
                this._refreshBuffAttribute(buff, buff.idCom.parent);
                GameLogger.DebugLog($"{buff.owner.idCom} [{buff.model}] 层数叠加: {beforeLayer} -> {afterLayer}");
                var attachLayer = afterLayer - beforeLayer;
                return attachLayer;
            }

            return 0;
        }

        /// <summary> 尝试移除buff </summary>
        public bool TryDetachBuff(GameEntityBase buffTarget, int buffId, int layer, out GameBuffEntity detachBuff, out int detachLayer)
        {
            detachBuff = null;
            detachLayer = 0;

            // 获取buff组件
            var buffCom = this._GetBuffCom(buffTarget);
            if (buffCom == null)
            {
                GameLogger.LogError("未处理的buff目标类型：" + buffTarget.idCom.entityType);
                return false;
            }

            if (!buffCom.TryGet(buffId, out detachBuff))
            {
                GameLogger.LogError("Buff不存在，无法移除：" + buffId);
                return false;
            }

            if (!detachBuff.isValid) return false;

            // 移除层数
            detachLayer = this._DetachLayer(detachBuff, layer);

            if (!detachBuff.isValid) buffCom.Remove(detachBuff);

            this._context.SubmitRC(GameBuffRCCollection.RC_GAME_BUFF_DETACH, new GameBuffRCArgs_Detach
            {
                buffId = buffId,
                targetIdArgs = buffTarget.idCom.ToArgs(),
                detachLayer = detachLayer,
            });

            return true;
        }

        /// <summary> 尝试移除buff </summary>
        public bool TryDetachBuff(GameEntityBase target, int buffId, int layer)
        {
            return this.TryDetachBuff(target, buffId, layer, out _, out _);
        }

        public void DetachAllBuff(GameEntityBase target)
        {
            if (!target.TryGetLinkParent<GameRoleEntity>(out var targetRole))
            {
                GameLogger.LogError("目标不是角色, 暂不支持移除Buff");
                return;
            }

            // 遍历buff列表, 移除所有buff
            var buffList = targetRole.buffCom.GetBuffList();
            buffList.Foreach(buff =>
            {
                this.TryDetachBuff(target, buff.model.typeId, 0);
            });

            // 清空buff列表
            targetRole.buffCom.Clear();
        }

        /// <summary> 执行移除层数 </summary>
        private int _DetachLayer(GameBuffEntity buff, int layer)
        {
            layer = layer == 0 ? buff.model.maxLayer : layer;
            var beforeLayer = buff.layer;
            var afterLayer = beforeLayer - layer;
            afterLayer = GameMath.Max(afterLayer, 0);
            buff.layer = afterLayer;

            var actor = buff.idCom.parent;
            this._refreshBuffAttribute(buff, actor);

            GameLogger.DebugLog($"{buff.owner.idCom} [{buff.model}] 层数移除: {beforeLayer} -> {afterLayer}");

            if (afterLayer <= 0 && buff.model.NeedRemoveAtZeroLayer())
            {
                buff.SetInvalid();
                GameLogger.DebugLog($"{buff.owner.idCom} [{buff.model}] 置为无效");
            }

            var detachLayer = beforeLayer - afterLayer;
            return detachLayer;
        }


        /// <summary> 刷新buff属性效果, 同时会刷新目标的属性 </summary>
        private void _refreshBuffAttribute(GameBuffEntity buff, GameEntityBase actor)
        {
            var targetEntity = buff.actionTargeterCom.getCurTargeter().targetEntity;
            if (!targetEntity)
            {
                GameLogger.LogError("Buff当前不存在选取目标, 无法刷新buff属性效果");
                return;
            }
            buff.model.attributeModels?.Foreach(attrModel =>
            {
                var args = GameActionUtil_AttributeModify.CalcAttributeModify(actor, targetEntity, attrModel, 0);
                var attrType = args.modifyType;

                // 记录buff属性效果旧值
                var buffOldValue = buff.ownEffect.GetValue(attrType);

                // 设置buff新的属性效果
                var newEffect = new GameAttribute(attrType, args.modifyValue);
                newEffect.value *= buff.layer;
                buff.ownEffect.SetAttribute(newEffect);

                // 刷新buff对目标角色的属性效果
                var roleOldValue = buff.owner.attributeCom.GetValue(attrType);
                var offset = newEffect.value - buffOldValue;
                GameActionUtil_AttributeModify.DoAttributeModify(buff.owner, attrType, offset);

                var roleNewValue = buff.owner.attributeCom.GetValue(attrType);
                GameLogger.DebugLog($"Buff属性效果: {attrType} {roleOldValue} -> {roleNewValue}");
            });
            buff.actionTargeterCom.UpdateTargeter();
        }

        public void TranserBuffCom(GameBuffCom refBuffCom, GameRoleEntity targetRole)
        {
            var buffList = targetRole.buffCom.buffList;
            buffList.Clear();
            buffList.AddRange(refBuffCom.buffList);
            buffList.Foreach(buff =>
            {
                buff.owner = targetRole;
            });
            refBuffCom.buffList.Clear();
        }

    }
}