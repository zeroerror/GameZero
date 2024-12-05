using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileTriggerEMSet
    {
        [Header("触发器 - 持续时间")]
        public GameProjectileTriggerEM_Duration durationTriggerModel;
        [Header("触发器 - 体积碰撞")]
        public GameProjectileTriggerEM_VolumeCollision volumeCollisionTriggerModel;
        [Header("触发器 - 与目标碰撞")]
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