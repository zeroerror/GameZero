using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileTriggerEM_VolumeCollision
    {
        [HideInInspector]
        public bool enable;
        [Header("触发行为列表")]
        public GameActionSO[] actionSOs;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;
        public GameEntitySelectorEM detectEntitySelectorEM;

        public GameProjectileTriggerModel_VolumeCollision ToModel()
        {
            if (!enable) return null;
            var actionIds = this.actionSOs?.Map(so => so.typeId);
            var model = new GameProjectileTriggerModel_VolumeCollision(
                actionIds,
                nextStateType,
                detectEntitySelectorEM.ToModel()
            );
            return model;
        }
    }
}