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
            // 检查
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            if (!entitySelectApi.CheckSelectorAnchor(actor, actionModel.selector))
            {
                return;
            }

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
                default:
                    GameLogger.LogError($"未处理的行为类型：{actionModel.GetType().Name}");
                    break;
            }

            // 提交RC - 行为执行
            var actAnchorPos = entitySelectApi.GetSelectorAnchorPosition(actor, actionModel.selector);
            var evArgs = new GameActionRCArgs_Do(
                actionModel.typeId,
                actor.idCom.ToArgs(),
                actAnchorPos
            );
            this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DO, evArgs);
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
            var optionEntity = optionRepo.FindByCampId(campId);
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

            optionEntity.AddLevel();

            optionModel.actionIds?.Foreach((actionId) =>
            {
                optionEntity.physicsCom.ClearCollided();
                this.DoAction(actionId, optionEntity, optionEntity.lv);
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
                    var isSuc = GameActionUtil_Dmg.DoDmg(selectedEntity, record);
                    if (!isSuc) return;
                    // 记录
                    this._actionContext.dmgRecordList.Add(record);
                    // 提交RC
                    var evArgs = new GameActionRCArgs_Dmg(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DMG, evArgs);
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
                    var evArgs = new GameActionRCArgs_Heal(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_HEAL, evArgs);
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
                    var evArgs = new GameActionRCArgs_LaunchProjectile(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, evArgs);
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
                    var evArgs = new GameActionRCArgs_KnockBack(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_KNOCK_BACK, evArgs);
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
                    if (!dontDo) GameActionUtil_AttributeModify.DoAttributeModify(selectedEntity, record);
                    // 记录
                    this._actionContext.attributeModifyRecordList.Add(record);
                    // 提交RC
                    var evArgs = new GameActionRCArgs_AttributeModify(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_ATTRIBUTE_MODIFY, evArgs);
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
                    var evArgs = new GameActionRCArgs_AttachBuff(
                        action.typeId,
                        record
                    );
                    this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_ATTACH_BUFF, evArgs);
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
                var evArgs = new GameActionRCArgs_SummonRoles(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_SUMMON_ROLE, evArgs);
            });
        }

        public List<GameActionOptionModel> GetActionOptionModelList()
        {
            return this._actionContext.optionTemplate.GetActionOptionModelList();
        }
    }
}