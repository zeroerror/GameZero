using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileTriggerEM_ImpactTarget
    {
        [Header("是否启用")]
        public bool enable;
        [Header("触发的行为")]
        public GameActionSO actionSO;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;

        [Header("用于检测的选择器")]
        public GameEntitySelectorEM checkSelectorEM;
        [Header("是否为检测目标碰撞体")]
        public bool checkByTargetCollider;

        public GameProjectileTriggerModel_ImpactTarget ToModel()
        {
            if (!enable) return null;
            var actionId = this.actionSO == null ? 0 : this.actionSO.typeId;
            var model = new GameProjectileTriggerModel_ImpactTarget(
                actionId,
                nextStateType,
                checkSelectorEM.ToSelector(),
                checkByTargetCollider
            );
            return model;
        }
    }
}