using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileStateTriggerEMSet
    {
        [Header("触发器 - 持续时间")]
        public GameProjectileStateTriggerEM_Duration durationTriggerModel;
        [Header("触发器 - 体积碰撞")]
        public GameProjectileStateTriggerEM_VolumeCollision volumeCollisionTriggerModel;
        [Header("触发器 - 与目标碰撞")]
        public GameProjectileStateTriggerEM_ImpactTarget impactTargetTriggerModel;

        public GameProjectileStateTriggerModelSet ToModelSet()
        {
            var modelSet = new GameProjectileStateTriggerModelSet(
                this.durationTriggerModel.ToModel(),
                this.volumeCollisionTriggerModel.ToModel(),
                this.impactTargetTriggerModel.ToModel()
            );
            return modelSet;
        }
    }
}