namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerDomain
    {
        GameContext _context;

        GameProjectileStateTriggerDomain_Duration _durationTriggerDomain;
        GameProjectileStateTriggerDomain_VolumeCollision _volumeCollisionTriggerDomain;

        public GameProjectileStateTriggerDomain()
        {
            this._durationTriggerDomain = new GameProjectileStateTriggerDomain_Duration();
            this._volumeCollisionTriggerDomain = new GameProjectileStateTriggerDomain_VolumeCollision();
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            this._durationTriggerDomain.Inject(context);
            this._volumeCollisionTriggerDomain.Inject(context);
            this._BindEvent();
        }

        public void Dispose()
        {
            this._UnbindEvents();
        }

        private void _BindEvent()
        {
        }

        private void _UnbindEvents()
        {
        }

        public void Tick(GameProjectileEntity entity, float dt)
        {
            var triggerCom = entity.triggerCom;
            var triggerSetDict = triggerCom.triggerSetDict;
            var triggerSet = triggerSetDict[entity.fsmCom.stateType];
            if (triggerSet == null) return;

            var actionApi = this._context.domainApi.actionApi;
            var durationTriggerModel = triggerSet.durationTriggerModel;
            if (durationTriggerModel != null)
            {
                var isSatisfied = this._durationTriggerDomain.CheckSatisfied(entity, durationTriggerModel, dt);
                if (isSatisfied) actionApi.DoAction(durationTriggerModel.actionId, entity);

            }
            var volumeCollisionTriggerModel = triggerSet.volumeCollisionTriggerModel;
            if (volumeCollisionTriggerModel != null)
            {
                var isSatisfied = this._volumeCollisionTriggerDomain.CheckSatisfied(entity, volumeCollisionTriggerModel, dt);
                if (isSatisfied) actionApi.DoAction(volumeCollisionTriggerModel.actionId, entity);
            }
        }
    }
}