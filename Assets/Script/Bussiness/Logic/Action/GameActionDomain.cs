using System.Collections.Generic;
using GamePlay.Core;

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

        public void DoAction(int actionId, GameEntityBase actor, int customParam)
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
                default:
                    GameLogger.LogError($"未处理的行为类型：{actionModel.GetType().Name}");
                    break;
            }

            // 提交RC - 行为执行
            var evArgs = new GameActionRCArgs_Do(
                actionModel.typeId,
                actor.idCom.ToArgs()
            );
            this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DO, evArgs);
        }

        public void DoAction_Dmg(GameActionModel_Dmg action, GameEntityBase actor)
        {
            var recordList = new List<GameActionRecord_Dmg>();
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.ForEach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                var record = GameActionUtil_Dmg.CalcDmg(actor, selectedEntity, action);
                var isSuc = GameActionUtil_Dmg.DoDmg(selectedEntity, record);
                if (isSuc) recordList.Add(record);
            });
            recordList.ForEach((record) =>
            {
                this._actionContext.dmgRecordList.Add(record);
                // 提交RC
                var evArgs = new GameActionRCArgs_Dmg(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DMG, evArgs);
            });
        }

        public void DoAction_Heal(GameActionModel_Heal action, GameEntityBase actor)
        {
            var recordList = new List<GameActionRecord_Heal>();
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.ForEach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;
                var record = GameActionHealUtil.CalcHeal(actor, selectedEntity, action);
                recordList.Add(record);
                GameActionHealUtil.DoHeal(selectedEntity, record);
            });
            recordList.ForEach((record) =>
            {
                this._actionContext.healRecordList.Add(record);
                // 提交RC
                var evArgs = new GameActionRCArgs_Heal(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_HEAL, evArgs);
            });
        }

        public void DoAction_LaunchProjectile(GameActionModel_LaunchProjectile action, GameEntityBase actor)
        {
            var recordList = new List<GameActionRecord_LaunchProjectile>();
            var projectileId = action.projectileId;
            var projectileApi = this._context.domainApi.projectileApi;
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.ForEach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                // 发射锚点位置
                var transArgs = selectedEntity.transformCom.ToArgs();
                // 发射偏移, 仅在x轴上对称翻转
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
                var record = new GameActionRecord_LaunchProjectile(
                    actionId: action.typeId,
                    actorRoleIdArgs: actor.TryGetLinkEntity<GameRoleEntity>()?.idCom.ToArgs() ?? default,
                    actorIdArgs: actor.idCom.ToArgs(),
                    targetRoleIdArgs: selectedEntity.idCom.ToArgs()
                );
                recordList.Add(record);
            });

            recordList.ForEach((record) =>
            {
                this._actionContext.launchProjectileRecordList.Add(record);
                // 提交RC
                var evArgs = new GameActionRCArgs_LaunchProjectile(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, evArgs);
            });
        }

        public void DoAction_KnockBack(GameActionModel_KnockBack action, GameEntityBase actor)
        {
            var recordList = new List<GameActionRecord_KnockBack>();
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.ForEach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                var record = GameActionKnockBackUtil.CalcKnockBack(actor, selectedEntity, action);
                recordList.Add(record);
                var transformApi = this._context.domainApi.transformApi;
                GameActionKnockBackUtil.DoKnockBack(selectedEntity, record, transformApi);
            });
            recordList.ForEach((record) =>
            {
                // 提交RC
                var evArgs = new GameActionRCArgs_KnockBack(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_KNOCK_BACK, evArgs);
            });
        }

        public void DoAction_AttributeModify(GameActionModel_AttributeModify action, GameEntityBase actor, bool dontDo = false)
        {
            var recordList = new List<GameActionRecord_AttributeModify>();
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.ForEach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                var record = GameActionUtil_AttributeModify.CalcAttributeModify(actor, selectedEntity, action);
                recordList.Add(record);
                if (!dontDo) GameActionUtil_AttributeModify.DoAttributeModify(selectedEntity, record);
            });
            recordList.ForEach((record) =>
            {
                // 提交RC
                var evArgs = new GameActionRCArgs_AttributeModify(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_ATTRIBUTE_MODIFY, evArgs);
            });
        }

        public void DoAction_AttachBuff(GameActionModel_AttachBuff action, GameEntityBase actor)
        {
            var recordList = new List<GameActionRecord_AttachBuff>();
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actor);
            selectedEntities?.ForEach((selectedEntity) =>
            {
                if (!action.preconditionSet.CheckSatisfied(selectedEntity)) return;

                var isSuc = this._context.domainApi.buffApi.TryAttachBuff(action.buffId, actor, selectedEntity, action.layer, out var realAttachLayer);
                if (isSuc) recordList.Add(new GameActionRecord_AttachBuff(
                    actionId: action.typeId,
                    actorRoleIdArgs: actor.TryGetLinkEntity<GameRoleEntity>()?.idCom.ToArgs() ?? default,
                    actorIdArgs: actor.idCom.ToArgs(),
                    targetRoleIdArgs: selectedEntity.idCom.ToArgs(),
                    buffId: action.buffId,
                    layer: realAttachLayer
                ));
            });
            recordList.ForEach((record) =>
            {
                // 提交RC
                var evArgs = new GameActionRCArgs_AttachBuff(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_ATTACH_BUFF, evArgs);
            });
        }

        public void DoAction_SummonRole(GameActionModel_SummonRoles action, GameEntityBase actor)
        {
            var recordList = new List<GameActionRecord_SummonRoles>();
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var roles = this._context.domainApi.roleApi.SummonRoles(action, actor, actor.transformCom.ToArgs());
            if (roles.HasData())
            {
                var record = new GameActionRecord_SummonRoles(
                    actionId: action.typeId,
                    actorRoleIdArgs: actor.TryGetLinkEntity<GameRoleEntity>()?.idCom.ToArgs() ?? default,
                    actorIdArgs: actor.idCom.ToArgs(),
                    roleId: action.roleId,
                    count: action.count,
                    campType: action.campType
                );
                recordList.Add(record);
            }
            recordList.ForEach((record) =>
            {
                // 提交RC
                var evArgs = new GameActionRCArgs_SummonRoles(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_SUMMON_ROLE, evArgs);
            });
        }

    }
}