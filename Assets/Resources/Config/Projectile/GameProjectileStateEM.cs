using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileStateEM
    {
        public GameProjectileStateType stateType;
        // ---------------------- 类型对应的状态模型 ----------------------
        public GameProjectileStateEM_Attach attachStateEM;
        public GameProjectileStateEM_FixedDirection fixedDirectionStateEM;
        public GameProjectileStateEM_LockOnEntity lockOnEntityStateEM;
        public GameProjectileStateEM_LockOnPosition lockOnPositionStateEM;
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