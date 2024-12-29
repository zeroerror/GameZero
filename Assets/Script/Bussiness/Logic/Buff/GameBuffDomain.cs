using System;
using GamePlay.Core;
using UnityEditor;

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
            this._context.roleContext.repo.ForeachAllEntities(role =>
            {
                this._TickBuff(role, dt);
            });
        }

        private void _TickBuff(GameRoleEntity role, float dt)
        {
            var buffCom = role.buffCom;
            for (int i = 0; i < buffCom.buffList.Count; i++)
            {
                var buff = buffCom.buffList[i];
                buff.Tick(dt);
                // 行为触发
                var isSatisfied_action = buff.conditionSetEntity_action.CheckSatisfied();
                if (isSatisfied_action)
                {
                    buff.model.actionIds?.Foreach(actionId =>
                    {
                        buff.physicsCom.ClearCollided();
                        buff.attributeCom.SetByCom(buff.idCom.parent.attributeCom);
                        this._context.domainApi.actionApi.DoAction(actionId, buff);
                    });
                }
                // 移除触发
                var isSatisfied_remove = buff.conditionSetEntity_remove.CheckSatisfied();
                if (isSatisfied_remove)
                {
                    this._context.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        this.TryDetachBuff(role, buff.model.typeId, 0);
                    });
                }
            }
        }

        /// <summary> 尝试挂载buff </summary>
        public bool TryAttachBuff(int typeId, GameEntityBase actor, GameEntityBase target, int layer, out int realAttachLayer)
        {
            realAttachLayer = 0;

            if (!target.TryGetLinkEntity<GameRoleEntity>(out var targetRole))
            {
                GameLogger.LogError("目标不是角色, 暂不支持挂载Buff");
                return false;
            }

            var buffCom = targetRole.buffCom;
            if (buffCom.TryGet(typeId, out var existBuff))
            {
                realAttachLayer = this._AttachBuff(existBuff, actor, targetRole, layer);
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
            newBuff.idCom.SetEntityId(this._buffContext.idService.FetchId());
            newBuff.idCom.SetParent(targetRole);
            // 组件绑定
            newBuff.BindTransformCom(targetRole.transformCom);

            // 注入buff条件所需delegate
            this._InjectBuffConditionDelegate(newBuff, actor, targetRole);

            buffCom.Add(newBuff);
            this._buffContext.repo.TryAdd(newBuff);
            realAttachLayer = this._AttachBuff(newBuff, actor, targetRole, layer);
            return true;
        }

        private void _InjectBuffConditionDelegate(GameBuffEntity buff, GameEntityBase actor, GameRoleEntity targetRole)
        {
            buff.conditionSetEntity_action.Inject(
                this._ForeachActionRecord_Dmg,
                this._ForeachActionRecord_Heal,
                this._ForeachActionRecord_LaunchProjectile

            );
        }
        private void _ForeachActionRecord_Dmg(in Action<GameActionRecord_Dmg> actionRecord)
        {
            this._context.actionContext.dmgRecordList.ForEach(actionRecord);
        }
        private void _ForeachActionRecord_Heal(in Action<GameActionRecord_Heal> actionRecord)
        {
            this._context.actionContext.healRecordList.ForEach(actionRecord);
        }
        private void _ForeachActionRecord_LaunchProjectile(in Action<GameActionRecord_LaunchProjectile> actionRecord)
        {
            this._context.actionContext.launchProjectileRecordList.ForEach(actionRecord);
        }

        /// <summary> 执行挂载buff </summary>
        private int _AttachBuff(GameBuffEntity buff, GameEntityBase actor, GameRoleEntity targetRole, int layer)
        {
            // 设置目标
            buff.target = targetRole;

            // 设置行为目标
            buff.actionTargeterCom.SetTargeter(new GameActionTargeterArgs
            {
                targetEntity = targetRole,
                targetPosition = targetRole.transformCom.position,
                targetDirection = targetRole.forward
            });

            // 挂载层数
            var realLayer = this._AttachLayer(buff, layer);
            // 刷新buff属性效果
            this._refreshBuffAttribute(buff, actor, targetRole);

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
        private int _AttachLayer(GameBuffEntity buff, int layer = 1)
        {
            var buffModel = buff.model;
            var beforeLayer = buff.layer;

            var refreshFlag = buffModel.refreshFlag;
            if (refreshFlag.HasFlag(GameBuffRefreshFlag.RefreshTime))
            {
                buff.conditionSetEntity_remove.RefreshTime();
            }
            if (refreshFlag.HasFlag(GameBuffRefreshFlag.StackTime))
            {
                buff.conditionSetEntity_remove.StackTime();
            }
            if (refreshFlag.HasFlag(GameBuffRefreshFlag.StackLayer))
            {
                var afterLayer = beforeLayer + layer;
                var maxLayer = buffModel.maxLayer == 0 ? int.MaxValue : buffModel.maxLayer;// 0表示无限层数
                afterLayer = GameMath.Min(afterLayer, maxLayer);
                buff.layer = afterLayer;
                var attachLayer = afterLayer - beforeLayer;
                GameLogger.DebugLog($"Buff层数变化: {beforeLayer} -> {afterLayer}");
                return attachLayer;
            }
            return 0;
        }

        /// <summary> 尝试移除buff </summary>
        public bool TryDetachBuff(GameEntityBase target, int buffId, int layer, out GameBuffEntity detachBuff, out int detachLayer)
        {
            detachBuff = null;
            detachLayer = 0;

            if (!target.TryGetLinkEntity<GameRoleEntity>(out var targetRole))
            {
                GameLogger.LogError("目标不是角色, 暂不支持移除Buff");
                return false;
            }

            var buffCom = targetRole.buffCom;
            detachBuff = buffCom.buffList.Find(b => b.model.typeId == buffId);
            if (!detachBuff)
            {
                GameLogger.LogError("Buff不存在，无法移除：" + buffId);
                return false;
            }

            if (!detachBuff.isValid) return false;

            // 移除层数
            detachLayer = this._DetachLayer(detachBuff, layer);
            // 刷新buff属性效果
            this._refreshBuffAttribute(detachBuff, targetRole, targetRole);

            if (!detachBuff.isValid) buffCom.buffList.Remove(detachBuff);

            this._context.SubmitRC(GameBuffRCCollection.RC_GAME_BUFF_DETACH, new GameBuffRCArgs_Detach
            {
                buffId = buffId,
                targetIdArgs = targetRole.idCom.ToArgs(),
                detachLayer = detachLayer,
            });

            return true;
        }

        /// <summary> 尝试移除buff </summary>
        public bool TryDetachBuff(GameEntityBase target, int buffId, int layer)
        {
            return this.TryDetachBuff(target, buffId, layer, out _, out _);
        }

        /// <summary> 执行移除层数 </summary>
        private int _DetachLayer(GameBuffEntity buff, int layer)
        {
            layer = layer == 0 ? buff.model.maxLayer : layer;

            var beforeLayer = buff.layer;
            var afterLayer = beforeLayer - layer;
            afterLayer = GameMath.Max(afterLayer, 0);
            buff.layer = afterLayer;
            if (afterLayer <= 0)
            {
                buff.SetInvalid();
            }
            var detachLayer = beforeLayer - afterLayer;
            GameLogger.DebugLog($"Buff层数变化: {beforeLayer} -> {afterLayer}");
            return detachLayer;
        }

        /// <summary> 刷新buff属性效果, 同时会刷新目标角色的属性 </summary>
        private void _refreshBuffAttribute(GameBuffEntity buff, GameEntityBase actor, GameRoleEntity targetRole)
        {
            buff.model.attributeModels?.Foreach(attrModel =>
            {
                var args = GameActionUtil_AttributeModify.CalcAttributeModify(actor, targetRole, attrModel);
                var attrType = args.modifyType;

                // 记录buff属性效果旧值
                var buffOldValue = buff.attributeCom.GetValue(attrType);

                // 设置buff新的属性效果
                var buffAttr = new GameAttribute(attrType, args.modifyValue);
                buffAttr.value *= buff.layer;
                buff.attributeCom.SetAttribute(buffAttr);

                // 刷新buff对目标角色的属性效果
                var roleOldValue = targetRole.attributeCom.GetValue(attrType);
                var roleNewValue = roleOldValue + buffAttr.value - buffOldValue;
                targetRole.attributeCom.SetAttribute(attrType, roleNewValue);

                GameLogger.DebugLog($"Buff属性效果: {attrType} {roleOldValue} -> {roleNewValue}");
            });
        }
    }
}