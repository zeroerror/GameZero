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
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var selectedEntities = entitySelectApi.SelectEntities(trigger.model.detectEntitySelector, projectile, false);
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