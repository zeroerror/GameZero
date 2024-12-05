using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerDomain_VolumeCollision
    {
        private GameContext _context;

        public GameProjectileTriggerDomain_VolumeCollision()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public bool CheckSatisfied(GameProjectileEntity projectile, GameProjectileTriggerEntity_VolumeCollision trigger, float dt)
        {
            // var triggerModel = trigger.model;
            // this._context.domainApi.actionApi.TryGetModel(triggerModel.actionId, out var actionModel);
            // var selector = actionModel?.selector;
            // if (selector == null || !selector.isRangeSelect)
            // {
            //     GameLogger.LogError("状态触发器[体积碰撞]: 需要包含一个范围选取行为");
            //     return false;
            // }

            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(trigger.model.checkSelector, projectile, false);
            if (selectedEntities == null || selectedEntities.Count == 0) return false;
            foreach (var entity in selectedEntities)
            {
                var t = new GameActionTargeterArgs();
                t.targetEntity = entity;
                t.targetPosition = entity.transformCom.position;
                t.targetDirection = entity.transformCom.position - projectile.transformCom.position;
            }
            return selectedEntities.Count > 0;
        }
    }
}