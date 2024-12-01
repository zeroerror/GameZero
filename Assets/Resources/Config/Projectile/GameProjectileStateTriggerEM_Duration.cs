using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileStateTriggerEM_Duration
    {
        [Header("持续时间")]
        public float duration;
        [Header("触发的行为")]
        public GameActionSO actionSO;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;

        public GameProjectileStateTriggerModel_Duration ToModel()
        {
            var actionId = this.actionSO == null ? 0 : this.actionSO.typeId;
            var model = new GameProjectileStateTriggerModel_Duration(actionId, nextStateType, duration);
            return model;
        }
    }
}