namespace GamePlay.Bussiness.Logic
{
    public struct GameProjectileBarrageModel_Spread
    {
        public readonly int count;
        public readonly int spreadAngle;

        public GameProjectileBarrageModel_Spread(int count, int spreadAngle)
        {
            this.count = count;
            this.spreadAngle = spreadAngle;
        }

        public GameProjectileBarrageModel_Spread GetCustomModel(int customParam)
        {
            var count = customParam * this.count;
            return new GameProjectileBarrageModel_Spread(
                count,
                spreadAngle
            );
        }
    }
}