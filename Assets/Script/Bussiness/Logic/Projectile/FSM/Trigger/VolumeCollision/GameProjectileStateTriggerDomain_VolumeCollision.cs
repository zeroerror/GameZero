using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerDomain_VolumeCollision
    {
        private GameContext _context;

        public GameProjectileStateTriggerDomain_VolumeCollision()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public bool CheckSatisfied(GameProjectileEntity projectile, GameProjectileStateTriggerEntity_VolumeCollision trigger, float dt)
        {
            var entitySelectApi = this._context.domainApi.entitySelectApi;
            var triggerModel = trigger.model;
            if (!this._context.domainApi.actionApi.TryGetModel(triggerModel.actionId, out var actionModel))
            {
                return false;
            }

            var selector = actionModel.selector;
            var selectedEntities = entitySelectApi.SelectEntities(selector, projectile, false);
            if (selectedEntities == null || selectedEntities.Count == 0) return false;
            var atc = projectile.actionTargeterCom;
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