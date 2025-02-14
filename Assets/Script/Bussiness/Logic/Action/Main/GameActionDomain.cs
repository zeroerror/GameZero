using System.Collections.Generic;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameActionDomain : GameActionDomainApi
    {
        GameContext _context;
        GameActionContext _actionContext => this._context.actionContext;
        public GameActionDomain()
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
            this._actionContext.ClearRecords();
        }

        public bool TryGetModel(int actionId, out GameActionModelBase model)
        {
            return this._context.actionContext.template.TryGet(actionId, out model);
        }

        public List<GameActionOptionModel> GetActionOptionModelList()
        {
            return this._actionContext.optionTemplate.GetActionOptionModelList();
        }

        public void DoAction(int actionId, GameEntityBase actor, float customParam)
        {
            var template = this._actionContext.template;
            if (!template.TryGet(actionId, out var actionModel))
            {
                GameLogger.LogError($"未找到行为配置：{actionId}");
                return;
            }

            // 获取自定义行为模型
            var customModel = actionModel.GetCustomModel(customParam);
            this._DoAction(actor, customModel);
        }

        public void DoAction(int actionId, GameEntityBase actor)
        {
            var template = this._actionContext.template;
            if (!template.TryGet(actionId, out var actionModel))
            {
                GameLogger.LogError($"未找到行为配置：{actionId}");
                return;
            }
            this._DoAction(actor, actionModel);
        }

        private void _DoAction(GameEntityBase actor, GameActionModelBase actionModel)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            switch (actionModel)
            {
                case GameActionModel_Dmg dmgAction:
                    this.DoAction_Dmg(dmgAction, actor);
                    break;
                case GameActionModel_Heal healAction:
                    this.DoAction_Heal(healAction, actor);
                    break;
                case GameActionModel_LaunchProjectile launchProjectileAction:
                    this.DoAction_LaunchProjectile(launchProjectileAction, actor);
                    break;
                case GameActionModel_KnockBack knockBackAction:
                    this.DoAction_KnockBack(knockBackAction, actor);
                    break;
                case GameActionModel_AttributeModify attributeModifyAction:
                    this.DoAction_AttributeModify(attributeModifyAction, actor);
                    break;
                case GameActionModel_AttachBuff attachBuffAction:
                    this.DoAction_AttachBuff(attachBuffAction, actor);
                    break;
                case GameActionModel_SummonRoles summonRolesAction:
                    this.DoAction_SummonRole(summonRolesAction, actor);
                    break;
                case GameActionModel_CharacterTransform characterTransformAction:
                    this.DoAction_CharacterTransform(characterTransformAction, actor);
                    break;
                case GameActionModel_Stealth stealthAction:
                    this.DoAction_Stealth(stealthAction, actor);
                    break;
                default:
                    GameLogger.LogError($"未处理的行为类型：{actionModel.GetType().Name}");
                    break;
            }

            // 提交RC - 行为执行
            var actAnchorPos = entitySelectApi.GetSelectorAnchorPosition(actor, actionModel.selector);
            var rcArgs = new GameActionRCArgs_Do(
                actionModel.typeId,
                actor.idCom.ToArgs(),
                actAnchorPos
            );
            this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DO, rcArgs);
        }

        public void DoActionOption(int optionId, int campId)
        {
            var optionTemplate = this._actionContext.optionTemplate;
            if (!optionTemplate.TryGet(optionId, out var optionModel))
            {
                GameLogger.LogError($"未找到选项配置：{optionId}");
                return;
            }

            var optionRepo = this._actionContext.optionRepo;
            var optionEntity = optionRepo.FindOption(campId, optionModel.typeId);
            if (!optionEntity)
            {
                // 新建选项实体
                optionEntity = new GameActionOptionEntity(optionModel);
                var idCom = optionEntity.idCom;
                idCom.entityId = this._actionContext.idService.FetchId();
                idCom.campId = campId;
                optionEntity.transformCom.position = GameVec2.zero;
                this._actionContext.optionRepo.TryAdd(optionEntity);
            }

            optionModel.actionIds?.Foreach((actionId) =>
            {
                optionEntity.physicsCom.ClearCollided();
                this.DoAction(actionId, optionEntity);
            });
        }

        public void DoAction_Dmg(GameActionModel_Dmg action, GameEntityBase actor)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.Foreach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                // 立即计算
                var record = GameActionUtil_Dmg.CalcDmg(actor, selectedEntity, action);
                // 帧末执行
                this._context.cmdBufferService.AddDelayCmd(0, () =>
                {
                    // 执行
                    GameActionUtil_Dmg.DoDmg(selectedEntity, ref record);
                    // 记录
                    this._actionContext.dmgRecordList.Add(record);
                    // 提交RC
                    var rcArgs = new GameActionRCArgs_Dmg(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DMG, rcArgs);
                });
            });
        }

        public void DoAction_Heal(GameActionModel_Heal action, GameEntityBase actor)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.Foreach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                // 立即计算
                var record = GameActionHealUtil.CalcHeal(actor, selectedEntity, action);
                // 帧末执行
                this._context.cmdBufferService.AddDelayCmd(0, () =>
                {
                    // 执行
                    GameActionHealUtil.DoHeal(selectedEntity, record);
                    // 记录
                    this._actionContext.healRecordList.Add(record);
                    // 提交RC
                    var rcArgs = new GameActionRCArgs_Heal(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_HEAL, rcArgs);
                });
            });
        }

        public void DoAction_LaunchProjectile(GameActionModel_LaunchProjectile action, GameEntityBase actor)
        {
            var projectileId = action.projectileId;
            var projectileApi = this._context.domainApi.projectileApi;
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.Foreach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                var targeter = actor.actionTargeterCom.getCurTargeter();
                // 帧末执行
                this._context.cmdBufferService.AddDelayCmd(0, () =>
                {
                    // 执行
                    // 发射锚点位置、偏移, 仅在x轴上对称翻转
                    var transArgs = selectedEntity.transformCom.ToArgs();
                    var launchOffet = action.launchOffset;
                    launchOffet.x = transArgs.forward.x > 0 ? launchOffet.x : -launchOffet.x;
                    transArgs.position += launchOffet;
                    var targeter = actor.actionTargeterCom.getCurTargeter();
                    switch (action.barrageType)
                    {
                        case GameProjectileBarrageType.None:
                            projectileApi.CreateProjectile(projectileId, actor, transArgs, targeter);
                            break;
                        case GameProjectileBarrageType.CustomLaunchOffset:
                            projectileApi.CreateBarrage(projectileId, actor, transArgs, targeter, action.customLaunchOffsetModel);
                            break;
                        case GameProjectileBarrageType.Spread:
                            projectileApi.CreateBarrage(projectileId, actor, transArgs, targeter, action.spreadModel);
                            break;
                        default:
                            GameLogger.LogError($"行为无法执行, 尚未处理的弹幕类型：{action.barrageType}");
                            break;
                    }
                    // 记录
                    var record = new GameActionRecord_LaunchProjectile(
                        actionId: action.typeId,
                        actorRoleIdArgs: actor.GetLinkParent<GameRoleEntity>()?.idCom.ToArgs() ?? default,
                        actorIdArgs: actor.idCom.ToArgs(),
                        targetRoleIdArgs: selectedEntity.idCom.ToArgs(),
                        actor.actionTargeterCom.getCurTargeterAsRecord()
                    );
                    this._actionContext.launchProjectileRecordList.Add(record);
                    // 提交RC
                    var rcArgs = new GameActionRCArgs_LaunchProjectile(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, rcArgs);
                });
            });
        }

        public void DoAction_KnockBack(GameActionModel_KnockBack action, GameEntityBase actor)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.Foreach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                // 立即计算
                var record = GameActionKnockBackUtil.CalcKnockBack(actor, selectedEntity, action);
                // 帧末执行
                this._context.cmdBufferService.AddDelayCmd(0, () =>
                {
                    // 执行
                    var transformApi = this._context.domainApi.transformApi;
                    GameActionKnockBackUtil.DoKnockBack(selectedEntity, record, transformApi);
                    // 记录
                    this._actionContext.knockBackRecordList.Add(record);
                    // 提交RC
                    var rcArgs = new GameActionRCArgs_KnockBack(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_KNOCK_BACK, rcArgs);
                });
            });
        }

        public void DoAction_AttributeModify(GameActionModel_AttributeModify action, GameEntityBase actor, bool dontDo = false)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.Foreach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                // 立即计算
                var record = GameActionUtil_AttributeModify.CalcAttributeModify(actor, selectedEntity, action);
                // 帧末执行
                this._context.cmdBufferService.AddDelayCmd(0, () =>
                {
                    // 执行
                    if (!dontDo) GameActionUtil_AttributeModify.DoAttributeModify(selectedEntity, record.modifyType, record.modifyValue);
                    // 记录
                    this._actionContext.attributeModifyRecordList.Add(record);
                    // 提交RC
                    var rcArgs = new GameActionRCArgs_AttributeModify(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_ATTRIBUTE_MODIFY, rcArgs);
                });
            });
        }

        public void DoAction_AttachBuff(GameActionModel_AttachBuff action, GameEntityBase actor)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.Foreach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                // 帧末执行
                this._context.cmdBufferService.AddDelayCmd(0, () =>
                {
                    // 执行
                    var isSuc = this._context.domainApi.buffApi.TryAttachBuff(action.buffId, actor, selectedEntity, action.layer, out var realAttachLayer);
                    if (!isSuc) return;
                    // 记录
                    var record = new GameActionRecord_AttachBuff(
                        actionId: action.typeId,
                        actorRoleIdArgs: actor.GetLinkParent<GameRoleEntity>()?.idCom.ToArgs() ?? default,
                        actorIdArgs: actor.idCom.ToArgs(),
                        targetRoleIdArgs: selectedEntity.idCom.ToArgs(),
                        actor.actionTargeterCom.getCurTargeterAsRecord(),
                        buffId: action.buffId,
                        layer: realAttachLayer
                    );
                    this._actionContext.attachBuffRecordList.Add(record);
                    // 提交RC
                    var rcArgs = new GameActionRCArgs_AttachBuff(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_ATTACH_BUFF, rcArgs);
                });
            });
        }

        public void DoAction_SummonRole(GameActionModel_SummonRoles action, GameEntityBase actor)
        {
            // 帧末执行
            this._context.cmdBufferService.AddDelayCmd(0, () =>
            {
                // 执行
                var roles = this._context.domainApi.roleApi.SummonRoles(action, actor, actor.transformCom.ToArgs());
                if (!roles.HasData()) return;
                // 记录
                var record = new GameActionRecord_SummonRoles(
                    actionId: action.typeId,
                    actorRoleIdArgs: actor.GetLinkParent<GameRoleEntity>()?.idCom.ToArgs() ?? default,
                    actorIdArgs: actor.idCom.ToArgs(),
                    roleId: action.roleId,
                    count: action.count,
                    campType: action.campType
                );
                this._actionContext.summonRolesRecordList.Add(record);
                // 提交RC
                var rcArgs = new GameActionRCArgs_SummonRoles(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_SUMMON_ROLE, rcArgs);
            });
        }

        public void DoAction_CharacterTransform(GameActionModel_CharacterTransform action, GameEntityBase actor)
        {
            var selector = action.selector;
            if (selector.selectAnchorType == GameEntitySelectAnchorType.Actor && !selector.IsRangeSelect())
            {
                // 锚点选中自己且为单选, 则代表将自身变身为自身当前选取的目标
                var targeter = actor.actionTargeterCom.getCurTargeter();
                var actTarget = targeter.targetEntity;
                if (!(actTarget is GameRoleEntity actTargetRole))
                {
                    GameLogger.LogError($"用于变身行为参考的行为目标不是角色实体：{actTarget.idCom.entityId}, 暂不支持");
                    return;
                }
                if (!actor.TryGetLinkParent<GameRoleEntity>(out var transTarget))
                {
                    GameLogger.Log("不存在需要变身的目标角色实体");
                    return;
                }

                this._CharacterTransform(actor, actTargetRole, action, transTarget, actTargetRole.model.typeId);
                return;
            }

            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.Foreach((selectedEntity) =>
            {
                this._CharacterTransform(actor, selectedEntity, action, selectedEntity, action.transToRoleId);
            });
        }

        /// <summary>
        /// 角色变身
        /// <para>actor: 行为发起者</para>
        /// <para>actTarget: 行为选取目标</para>
        /// <para>action: 行为模型</para>
        /// <para>beTransTarget: 需要受到变身的目标</para>
        /// <para>transToRoleId: 要变身成的角色ID</para>
        /// </summary>
        private void _CharacterTransform(
            GameEntityBase actor,
            GameEntityBase actTarget,
            GameActionModel_CharacterTransform action,
            GameEntityBase beTransTarget,
            int transToRoleId)
        {
            if (!action.preconditionSet.CheckSatisfied(beTransTarget)) return;
            if (!(beTransTarget is GameRoleEntity transTargetRole))
            {
                GameLogger.LogError($"变身目标不是角色实体：{beTransTarget.idCom}, 暂不支持");
                return;
            }

            // 立即计算
            var record = GameActionUtil_CharacterTransform.CalcCharacterTransform(actor, actTarget, action);
            // 帧末执行
            this._context.cmdBufferService.AddDelayCmd(0, () =>
            {
                // 执行
                this._context.domainApi.roleApi.TransformRole(transTargetRole, transToRoleId, record);
                // 记录
                this._actionContext.transformRecordList.Add(record);
                // 提交RC 
                GameActionRCArgs_CharacterTransform rcArgs;
                rcArgs.actionId = action.typeId;
                rcArgs.record = record;
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_TRANSFORM, rcArgs);
            });
        }

        public void DoAction_Stealth(GameActionModel_Stealth action, GameEntityBase actor)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.Foreach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;
                // 帧末执行
                this._context.cmdBufferService.AddDelayCmd(0, () =>
                {
                    // 执行
                    var record = GameActionUtil_Stealth.CalcStealth(actor, selectedEntity, action);
                    GameActionUtil_Stealth.DoStealth(selectedEntity, record, this._context.domainApi);
                    // 记录
                    this._actionContext.stealthRecordList.Add(record);
                    // 提交RC
                    var rcArgs = new GameActionRCArgs_Stealth(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_STEALTH, rcArgs);
                });
            });
        }
    }
}