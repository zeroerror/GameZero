using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileTriggerEM_Duration
    {
        [HideInInspector]
        public bool enable;
        [Header("持续时间")]
        public float duration;

        [Header("触发的行为")]
        public GameActionSO actionSO;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;

        public GameProjectileTriggerModel_Duration ToModel()
        {
            if (!enable) return null;
            var actionId = this.actionSO == null ? 0 : this.actionSO.typeId;
            var model = new GameProjectileTriggerModel_Duration(actionId, nextStateType, duration);
            return model;
        }
    }
}