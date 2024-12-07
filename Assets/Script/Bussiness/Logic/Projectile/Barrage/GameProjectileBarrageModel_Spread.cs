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
    }
}