using GamePlay.Bussiness.Logic;
using GamePlay.Core;
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
        [Header("触发行为列表")]
        public GameActionSO[] actionSOs;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;

        public GameProjectileTriggerModel_Duration ToModel()
        {
            if (!enable) return null;
            var actionIds = this.actionSOs?.Map(so => so.typeId);
            var model = new GameProjectileTriggerModel_Duration(actionIds, nextStateType, duration);
            return model;
        }
    }
}