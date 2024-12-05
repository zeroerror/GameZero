using GamePlay.Bussiness.Logic;
using GamePlay.Config;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileTriggerEM_VolumeCollision
    {
        [Header("触发的行为")]
        public GameActionSO actionSO;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;

        [Header("用于检测的选择器")]
        public GameEntitySelectorEM checkSelectorEM;

        public GameProjectileTriggerModel_VolumeCollision ToModel()
        {
            if (nextStateType == GameProjectileStateType.None) return null;
            var actionId = this.actionSO == null ? 0 : this.actionSO.typeId;
            var model = new GameProjectileTriggerModel_VolumeCollision(
                actionId,
                nextStateType,
                checkSelectorEM.ToSelector()
            );
            return model;
        }
    }
}