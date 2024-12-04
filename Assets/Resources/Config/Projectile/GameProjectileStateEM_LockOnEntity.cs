using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public struct GameProjectileStateEM_LockOnEntity
    {
        [Header("目标选取类型")]
        public GameProjectileTargeterType targeterType;
        [Header("速度")]
        public float speed;
        [Header("一定时间抵达")]
        public float timeToArrive;

        public GameProjectileStateModel_LockOnEntity ToModel()
        {
            return new GameProjectileStateModel_LockOnEntity(targeterType, speed, timeToArrive);
        }
    }
}