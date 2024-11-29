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

        public void DoAction(int actionId, GameEntityBase actorEntity)
        {
            var template = this._actionContext.template;
            if (!template.TryGet(actionId, out var actionModel))
            {
                GameLogger.LogError($"未找到行为配置：{actionId}");
                return;
            }
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.GetSelectdeEntities(actionModel.selector, actorEntity);
            selectedEntities.ForEach((entity) =>
            {
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
                    default:
                        GameLogger.LogError($"未实现的行为类型：{actionModel.GetType().Name}");
                        break;
                }
            });
        }

        public void DoAction_Dmg(GameActionModel_Dmg action, GameEntityBase actorEntity)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.GetSelectdeEntities(action.selector, actorEntity);
            selectedEntities.ForEach((entity) =>
            {
                GameLogger.Log($"执行行为[伤害]: {action}");
            });
        }

        public void DoAction_Heal(GameActionModel_Heal action, GameEntityBase actorEntity)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.GetSelectdeEntities(action.selector, actorEntity);
            selectedEntities.ForEach((entity) =>
            {
                GameLogger.Log($"执行行为[治疗]: {action}");
            });
        }

        public void DoAction_LaunchProjectile(GameActionModel_LaunchProjectile action, GameEntityBase actorEntity)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.GetSelectdeEntities(action.selector, actorEntity);
            selectedEntities.ForEach((entity) =>
            {
                GameLogger.Log($"执行行为[发射弹体]: {action}");
            });
        }
    }
}