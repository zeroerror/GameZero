using GamePlay.Bussiness.Logic;
using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileStateTriggerEM_VolumeCollision
    {
        [Header("触发的行为")]
        public GameActionSO actionSO;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;

        public GameProjectileStateTriggerModel_VolumeCollision ToModel()
        {
            var actionId = this.actionSO == null ? 0 : this.actionSO.typeId;
            var model = new GameProjectileStateTriggerModel_VolumeCollision(actionId, nextStateType);
            return model;
        }
    }
}