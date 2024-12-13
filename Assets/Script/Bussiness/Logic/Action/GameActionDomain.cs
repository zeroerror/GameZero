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

        public void Dispose()
        {
        }

        public void Tick(float dt)
        {
        }

        public bool TryGetModel(int actionId, out GameActionModelBase model)
        {
            return this._context.actionContext.template.TryGet(actionId, out model);
        }

        public void DoAction(int actionId, GameEntityBase actorEntity)
        {
            var template = this._actionContext.template;
            if (!template.TryGet(actionId, out var actionModel))
            {
                GameLogger.LogError($"未找到行为配置：{actionId}");
                return;
            }
            switch (actionModel)
            {
                case GameActionModel_Dmg dmgAction:
                    this.DoAction_Dmg(dmgAction, actorEntity);
                    break;
                case GameActionModel_Heal healAction:
                    this.DoAction_Heal(healAction, actorEntity);
                    break;
                case GameActionModel_LaunchProjectile launchProjectileAction:
                    this.DoAction_LaunchProjectile(launchProjectileAction, actorEntity);
                    break;
                case GameActionModel_KnockBack knockBackAction:
                    this.DoAction_KnockBack(knockBackAction, actorEntity);
                    break;
                case GameActionModel_Attribute attributeModifyAction:
                    this.DoAction_AttributeModify(attributeModifyAction, actorEntity);
                    break;
                default:
                    GameLogger.LogError($"未处理的行为类型：{actionModel.GetType().Name}");
                    break;
            }
        }

        public void DoAction_Dmg(GameActionModel_Dmg action, GameEntityBase actorEntity)
        {
            var recordList = new List<GameActionRecord_Dmg>();
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actorEntity);
            selectedEntities?.ForEach((selectedEntity) =>
            {
                var record = GameActionUtil_Dmg.CalcDmg(actorEntity, selectedEntity, action);
                recordList.Add(record);
                GameActionUtil_Dmg.DoDmg(selectedEntity, record);
            });
            recordList.ForEach((record) =>
            {
                this._actionContext.dmgRecordList.Add(record);
                // 提交RC
                var evArgs = new GameActionRCArgs_DoDmg(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DO_DMG, evArgs);
            });
        }

        public void DoAction_Heal(GameActionModel_Heal action, GameEntityBase actorEntity)
        {
            var recordList = new List<GameActionRecord_Heal>();
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actorEntity);
            selectedEntities?.ForEach((selEntity) =>
            {
                var record = GameActionHealUtil.CalcHeal(actorEntity, selEntity, action);
                recordList.Add(record);
                GameActionHealUtil.DoHeal(selEntity, record);
            });
            recordList.ForEach((record) =>
            {
                this._actionContext.healRecordList.Add(record);
                // 提交RC
                var evArgs = new GameActionRCArgs_DoHeal(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DO_HEAL, evArgs);
            });
        }

        public void DoAction_LaunchProjectile(GameActionModel_LaunchProjectile action, GameEntityBase actorEntity)
        {
            var recordList = new List<GameActionRecord_LaunchProjectile>();
            var projectileId = action.projectileId;
            var projectileApi = this._context.domainApi.projectileApi;
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actorEntity);
            selectedEntities?.ForEach((selectedEntity) =>
            {
                // 发射锚点位置
                var transArgs = selectedEntity.transformCom.ToArgs();
                // 发射偏移, 仅在x轴上对称翻转
                var launchOffet = action.launchOffset;
                launchOffet.x = transArgs.forward.x > 0 ? launchOffet.x : -launchOffet.x;
                transArgs.position += launchOffet;
                var targeter = actorEntity.actionTargeterCom.getCurTargeter();
                switch (action.barrageType)
                {
                    case GameProjectileBarrageType.None:
                        projectileApi.CreateProjectile(projectileId, actorEntity, transArgs, targeter);
                        break;
                    case GameProjectileBarrageType.CustomLaunchOffset:
                        projectileApi.CreateBarrage(projectileId, actorEntity, transArgs, targeter, action.customLaunchOffsetModel);
                        break;
                    case GameProjectileBarrageType.Spread:
                        projectileApi.CreateBarrage(projectileId, actorEntity, transArgs, targeter, action.spreadModel);
                        break;
                    default:
                        GameLogger.LogError($"行为无法执行, 尚未处理的弹幕类型：{action.barrageType}");
                        break;
                }
                var record = new GameActionRecord_LaunchProjectile(
                    actorRoleIdArgs: actorEntity.idCom.ToArgs(),
                    actorIdArgs: actorEntity.idCom.ToArgs(),
                    targetRoleIdArgs: selectedEntity.idCom.ToArgs()
                );
                recordList.Add(record);
            });

            recordList.ForEach((record) =>
            {
                // 提交RC
                var evArgs = new GameActionRCArgs_LaunchProjectile(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_LAUNCH_PROJECTILE, evArgs);
            });
        }

        public void DoAction_KnockBack(GameActionModel_KnockBack action, GameEntityBase actorEntity)
        {
            var recordList = new List<GameActionRecord_KnockBack>();
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actorEntity);
            selectedEntities?.ForEach((selectedEntity) =>
            {
                var record = GameActionKnockBackUtil.CalcKnockBack(actorEntity, selectedEntity, action);
                recordList.Add(record);
                var transformApi = this._context.domainApi.transformApi;
                GameActionKnockBackUtil.DoKnockBack(selectedEntity, record, transformApi);
            });
            recordList.ForEach((record) =>
            {
                // 提交RC
                var evArgs = new GameActionRCArgs_DoKnockBack(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DO_KNOCK_BACK, evArgs);
            });
        }

        public void DoAction_AttributeModify(GameActionModel_Attribute action, GameEntityBase actorEntity, bool dontDo = false)
        {
            var recordList = new List<GameActionRecord_AttributeModify>();
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actorEntity);
            selectedEntities?.ForEach((selectedEntity) =>
            {
                var record = GameActionUtil_AttributeModify.CalcAttributeModify(actorEntity, selectedEntity, action);
                recordList.Add(record);
                if (!dontDo) GameActionUtil_AttributeModify.DoAttributeModify(selectedEntity, record);
            });
            recordList.ForEach((record) =>
            {
                // 提交RC
                var evArgs = new GameActionRCArgs_DoAttributeModify(
                    action.typeId,
                    record
                );
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DO_ATTRIBUTE_MODIFY, evArgs);
            });
        }
    }
}