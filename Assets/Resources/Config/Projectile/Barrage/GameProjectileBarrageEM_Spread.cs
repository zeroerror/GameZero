using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameProjectileBarrageEM_Spread
    {
        [Range(1, 100), Header("散射数量"), Space(10)]
        public int count;
        [Range(0, 360)]
        public int spreadAngle;

        public GameProjectileBarrageModel_Spread ToModel()
        {
            GameProjectileBarrageModel_Spread model = new GameProjectileBarrageModel_Spread(
                this.count,
                this.spreadAngle
            );
            return model;
        }
    }
}