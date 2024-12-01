using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine.SocialPlatforms.Impl;

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

        public List<GameActionRecord> DoAction(int actionId, GameEntityBase actorEntity)
        {
            var template = this._actionContext.template;
            if (!template.TryGet(actionId, out var actionModel))
            {
                GameLogger.LogError($"未找到行为配置：{actionId}");
                return null;
            }
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            List<GameActionRecord> recordList = null;
            switch (actionModel)
            {
                case GameActionModel_Dmg dmgAction:
                    recordList = this.DoAction_Dmg(dmgAction, actorEntity);
                    break;
                case GameActionModel_Heal healAction:
                    recordList = this.DoAction_Heal(healAction, actorEntity);
                    break;
                case GameActionModel_LaunchProjectile launchProjectileAction:
                    recordList = this.DoAction_LaunchProjectile(launchProjectileAction, actorEntity);
                    break;
                default:
                    GameLogger.LogError($"未实现的行为类型：{actionModel.GetType().Name}");
                    break;
            }

            recordList?.ForEach((record) =>
            {
                this._actionContext.AddRecord(record);
                // 提交RC
                var evArgs = new GameActionRCArgs_Do()
                {
                    actionId = actionId,
                    actorIdArgs = actorEntity.idCom.ToArgs(),
                    targetIdArgs = record.targetIdArgs
                };
                this._context.SubmitRC(GameActionRCCollection.RC_GAME_ACTION_DO, evArgs);
            });
            return recordList;
        }

        public List<GameActionRecord> DoAction_Dmg(GameActionModel_Dmg action, GameEntityBase actorEntity)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actorEntity);
            var recordList = new List<GameActionRecord>();
            selectedEntities?.ForEach((selEntity) =>
            {
                var record = new GameActionRecord();
                record.actionId = action.typeId;
                record.actorIdArgs = actorEntity.idCom.ToArgs();
                record.targetIdArgs = selEntity.idCom.ToArgs();
                //todo 其余行为记录数据
                recordList.Add(record);
                GameLogger.Log($"执行行为[伤害]: {record}");
            });
            return recordList;
        }

        public List<GameActionRecord> DoAction_Heal(GameActionModel_Heal action, GameEntityBase actorEntity)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actorEntity);
            var recordList = new List<GameActionRecord>();
            selectedEntities?.ForEach((selEntity) =>
            {
                GameLogger.Log($"执行行为[治疗]: {action}");
                var record = new GameActionRecord();
                record.actionId = action.typeId;
                record.actorIdArgs = actorEntity.idCom.ToArgs();
                record.targetIdArgs = selEntity.idCom.ToArgs();
                //todo 其余行为记录数据
                recordList.Add(record);
            });
            return recordList;
        }

        public List<GameActionRecord> DoAction_LaunchProjectile(GameActionModel_LaunchProjectile action, GameEntityBase actorEntity)
        {
            var projectileId = action.projectileId;
            var projectileApi = this._context.domainApi.projectileApi;
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(action.selector, actorEntity);
            var recordList = new List<GameActionRecord>();
            selectedEntities?.ForEach((selEntity) =>
            {
                GameLogger.Log($"执行行为[发射投射物]: {action}");
                var projectile = projectileApi.CreateProjectile(projectileId, actorEntity, selEntity.transformCom.ToArgs());
                projectile.attributeCom.SetAttribute(GameAttributeType.MoveSpeed, action.speed);

                var record = new GameActionRecord();
                record.actionId = action.typeId;
                record.actorIdArgs = actorEntity.idCom.ToArgs();
                record.targetIdArgs = selEntity.idCom.ToArgs();
                recordList.Add(record);
            });
            return recordList;
        }

        public bool TryGetModel(int actionId, out GameActionModelBase model)
        {
            return this._context.actionContext.template.TryGet(actionId, out model);
        }
    }
}