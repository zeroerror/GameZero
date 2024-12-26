using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileTriggerEMSet
    {
        public GameProjectileTriggerEM_Duration durationTriggerModel;
        public GameProjectileTriggerEM_VolumeCollision volumeCollisionTriggerModel;
        public GameProjectileTriggerEM_ImpactTarget impactTargetTriggerModel;

        public GameProjectileTriggerModelSet ToModelSet()
        {
            var modelSet = new GameProjectileTriggerModelSet(
                this.durationTriggerModel?.ToModel(),
                this.volumeCollisionTriggerModel?.ToModel(),
                this.impactTargetTriggerModel?.ToModel()
            );
            return modelSet;
        }

        public bool HasRefAction(int actionId)
        {
            var find = !!durationTriggerModel.actionSOs?.Find(so => so.typeId == actionId);
            if (find) return true;
            find = !!volumeCollisionTriggerModel.actionSOs?.Find(so => so.typeId == actionId);
            if (find) return true;
            find = !!impactTargetTriggerModel.actionSOs?.Find(so => so.typeId == actionId);
            if (find) return true;
            return false;
        }
    }
}