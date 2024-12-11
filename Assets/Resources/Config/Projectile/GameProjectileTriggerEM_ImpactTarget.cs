using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileTriggerEM_ImpactTarget
    {
        [HideInInspector]
        public bool enable;

        [Header("是否为检测目标碰撞体")]
        public bool checkByTargetCollider;
        [Header("触发行为列表")]
        public GameActionSO[] actionSOs;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;
        public GameEntitySelectorEM detectEntitySelectorEM;

        public GameProjectileTriggerModel_ImpactTarget ToModel()
        {
            if (!enable) return null;
            var actionIds = this.actionSOs?.Map(so => so.typeId);
            var model = new GameProjectileTriggerModel_ImpactTarget(
                actionIds,
                nextStateType,
                detectEntitySelectorEM.ToSelector(),
                checkByTargetCollider
            );
            return model;
        }
    }
}