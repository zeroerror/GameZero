using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileStateTriggerEMSet
    {
        [Header("持续时间触发器")]
        public GameProjectileStateTriggerEM_Duration durationTriggerModel;
        [Header("体积碰撞触发器")]
        public GameProjectileStateTriggerEM_VolumeCollision volumeCollisionTriggerModel;

        public GameProjectileStateTriggerModelSet ToModelSet()
        {
            var modelSet = new GameProjectileStateTriggerModelSet();
            modelSet.durationTriggerModel = this.durationTriggerModel.ToModel();
            modelSet.volumeCollisionTriggerModel = this.volumeCollisionTriggerModel.ToModel();
            return modelSet;
        }
    }
}