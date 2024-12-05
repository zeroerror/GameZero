using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    public class GameProjectileBarrageEM_Spread
    {
        public readonly int count;
        public readonly int spreadAngle;

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