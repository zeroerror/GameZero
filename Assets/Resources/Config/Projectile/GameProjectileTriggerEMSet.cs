using GamePlay.Bussiness.Logic;
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
    }
}