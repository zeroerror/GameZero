using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileTriggerEM_VolumeCollision
    {
        [HideInInspector]
        public bool enable;
        [Header("触发行为")]
        public GameActionSO actionSO;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;
        public GameEntitySelectorEM detectEntitySelectorEM;

        public GameProjectileTriggerModel_VolumeCollision ToModel()
        {
            if (!enable) return null;
            var actionId = this.actionSO == null ? 0 : this.actionSO.typeId;
            var model = new GameProjectileTriggerModel_VolumeCollision(
                actionId,
                nextStateType,
                detectEntitySelectorEM.ToSelector()
            );
            return model;
        }
    }
}