using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileTriggerEM_ImpactTarget
    {
        [HideInInspector]
        public bool enable;

        [Header("用于检测的实体选择器")]
        public GameEntitySelectorEM detectEntitySelectorEM;
        [Header("是否为检测目标碰撞体")]
        public bool checkByTargetCollider;

        [Header("触发的行为")]
        public GameActionSO actionSO;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;

        public GameProjectileTriggerModel_ImpactTarget ToModel()
        {
            if (!enable) return null;
            var actionId = this.actionSO == null ? 0 : this.actionSO.typeId;
            var model = new GameProjectileTriggerModel_ImpactTarget(
                actionId,
                nextStateType,
                detectEntitySelectorEM.ToSelector(),
                checkByTargetCollider
            );
            return model;
        }
    }
}