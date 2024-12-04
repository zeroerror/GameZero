using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public struct GameProjectileStateEM_FixedDirection
    {
        [Header("速度")]
        public float speed;

        public GameProjectileStateModel_FixedDirection ToModel()
        {
            return new GameProjectileStateModel_FixedDirection(speed);
        }
    }
}