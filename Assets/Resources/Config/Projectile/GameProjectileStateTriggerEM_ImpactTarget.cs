using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileStateTriggerEM_ImpactTarget
    {
        [Header("触发的行为")]
        public GameActionSO actionSO;
        [Header("下一个状态")]
        public GameProjectileStateType nextStateType;
        [Header("是否为检测目标碰撞体")]
        public bool checkByTargetCollider;

        public GameProjectileStateTriggerModel_ImpactTarget ToModel()
        {
            if (nextStateType == GameProjectileStateType.None) return null;
            var actionId = this.actionSO == null ? 0 : this.actionSO.typeId;
            var model = new GameProjectileStateTriggerModel_ImpactTarget(actionId, nextStateType, checkByTargetCollider);
            return model;
        }
    }
}