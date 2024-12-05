using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileStateEM
    {
        [Header("状态类型")]
        public GameProjectileStateType stateType;
        // ---------------------- 类型对应的状态模型 ----------------------
        [Header("状态模型 - 附着")]
        public GameProjectileStateEM_Attach attachStateEM;
        [Header("状态模型 - 固定方向")]
        public GameProjectileStateEM_FixedDirection fixedDirectionStateEM;
        [Header("状态模型 - 锁定实体")]
        public GameProjectileStateEM_LockOnEntity lockOnEntityStateEM;
        [Header("状态模型 - 锁定位置")]
        public GameProjectileStateEM_LockOnPosition lockOnPositionStateEM;

        [Header("触发器集合")]
        public GameProjectileTriggerEMSet emSet;

        public object ToModel()
        {
            switch (stateType)
            {
                case GameProjectileStateType.Attach:
                    return attachStateEM.ToModel();
                case GameProjectileStateType.FixedDirection:
                    return fixedDirectionStateEM.ToModel();
                case GameProjectileStateType.LockOnEntity:
                    return lockOnEntityStateEM.ToModel();
                case GameProjectileStateType.LockOnPosition:
                    return lockOnPositionStateEM.ToModel();
                default:
                    return null;
            }
        }
    }
}