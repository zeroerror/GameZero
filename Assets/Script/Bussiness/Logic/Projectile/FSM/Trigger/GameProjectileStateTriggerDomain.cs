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
        }

        public void Dispose()
        {
        }

        public GameProjectileStateType Tick(GameProjectileEntity entity, float dt)
        {
            var nextStateType = GameProjectileStateType.None;
            var triggerSetEntityDict = entity.fsmCom.triggerSetEntityDict;
            if (!triggerSetEntityDict.TryGetValue(entity.fsmCom.stateType, out var triggerSetEntity)) return nextStateType;
            var actionApi = this._context.domainApi.actionApi;

            {
                var triggerEntity = triggerSetEntity.durationTriggerEntity;
                if (triggerEntity != null)
                {
                    var isSatisfied = this._durationTriggerDomain.CheckSatisfied(entity, triggerEntity, dt);
                    if (isSatisfied)
                    {
                        var triggerModel = triggerEntity.model;
                        if (triggerModel.actionId != 0) actionApi.DoAction(triggerModel.actionId, entity);
                        if (triggerModel.nextStateType != GameProjectileStateType.None) nextStateType = triggerModel.nextStateType;
                    }
                }
            }
            {
                var triggerEntity = triggerSetEntity.volumeCollisionTriggerEntity;
                if (triggerEntity != null)
                {
                    var isSatisfied = this._volumeCollisionTriggerDomain.CheckSatisfied(entity, triggerEntity, dt);
                    if (isSatisfied)
                    {
                        var triggerModel = triggerEntity.model;
                        if (triggerModel.actionId != 0) actionApi.DoAction(triggerModel.actionId, entity);
                        if (triggerModel.nextStateType != GameProjectileStateType.None) nextStateType = triggerModel.nextStateType;
                    }
                }
            }

            return nextStateType;
        }
    }
}