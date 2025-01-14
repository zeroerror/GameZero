using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public struct GameProjectileStateEM_FixedDirection
    {
        [Header("速度")]
        public float speed;

        [Header("反弹次数, 大于0时开启反弹机制")]
        public int bounceCount;
        [Header("用于检测的实体选择器")]
        public GameEntitySelectorEM detectSelector;

        public GameProjectileStateModel_FixedDirection ToModel()
        {
            return new GameProjectileStateModel_FixedDirection(speed, bounceCount, detectSelector.ToModel());
        }
    }
}