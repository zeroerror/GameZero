using GamePlay.Config;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerDomain
    {
        GameContext _context;

        GameProjectileTriggerDomain_Duration _durationTriggerDomain;
        GameProjectileTriggerDomain_VolumeCollision _volumeCollisionTriggerDomain;
        GameProjectileTriggerDomain_ImpactTarget _impactTargetTriggerDomain;

        public GameProjectileTriggerDomain()
        {
            this._durationTriggerDomain = new GameProjectileTriggerDomain_Duration();
            this._volumeCollisionTriggerDomain = new GameProjectileTriggerDomain_VolumeCollision();
            this._impactTargetTriggerDomain = new GameProjectileTriggerDomain_ImpactTarget();
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            this._durationTriggerDomain.Inject(context);
            this._volumeCollisionTriggerDomain.Inject(context);
            this._impactTargetTriggerDomain.Inject(context);
        }

        public void Destroy()
        {
        }

        public void InitFSMTrigger(GameProjectileEntity projectile)
        {
            var fsmCom = projectile.fsmCom;
            foreach (var kv in projectile.model.triggerSetDict)
            {
                var stateType = kv.Key;
                var triggerSet = kv.Value;
                var durationEntity = triggerSet.durationTriggerModel == null ? null : new GameProjectileTriggerEntity_Duration(triggerSet.durationTriggerModel);
                var volumeCollisionEntity = triggerSet.volumeCollisionTriggerModel == null ? null : new GameProjectileTriggerEntity_VolumeCollision(triggerSet.volumeCollisionTriggerModel);
                var impactTargetEntity = triggerSet.impactTargetTriggerModel == null ? null : new GameProjectileTriggerEntity_ImpactTarget(triggerSet.impactTargetTriggerModel);
                var triggerSetEntity = new GameProjectileTriggerSetEntity(durationEntity, volumeCollisionEntity, impactTargetEntity);
                projectile.fsmCom.triggerSetEntityDict.Add(stateType, triggerSetEntity);
                if (fsmCom.defaultStateType == GameProjectileStateType.None) fsmCom.defaultStateType = stateType;
            }
            if (fsmCom.defaultStateType == GameProjectileStateType.None) fsmCom.defaultStateType = GameProjectileStateType.Idle;
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
                        triggerModel.actionIds.Foreach(actionId => actionApi.DoAction(actionId, entity));
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
                        triggerModel.actionIds.Foreach(actionId => actionApi.DoAction(actionId, entity));
                        if (triggerModel.nextStateType != GameProjectileStateType.None) nextStateType = triggerModel.nextStateType;
                    }
                }
            }
            {
                var triggerEntity = triggerSetEntity.impactTargetTriggerEntity;
                if (triggerEntity != null)
                {
                    var isSatisfied = this._impactTargetTriggerDomain.CheckSatisfied(entity, triggerEntity, dt);
                    if (isSatisfied)
                    {
                        var triggerModel = triggerEntity.model;
                        triggerModel.actionIds.Foreach(actionId => actionApi.DoAction(actionId, entity));
                        if (triggerModel.nextStateType != GameProjectileStateType.None) nextStateType = triggerModel.nextStateType;
                    }
                }
            }

            return nextStateType;
        }
    }
}