namespace GamePlay.Bussiness.Logic
{
    public struct GameProjectileStateModel_FixedDirection
    {
        public readonly float speed;

        /// <summary> 反弹次数, 大于0时开启反弹机制 </summary>
        public readonly int bounceCount;
        public readonly GameEntitySelector checkEntitySelector;

        public GameProjectileStateModel_FixedDirection(
            float speed,
            int bounceCount,
            GameEntitySelector checkEntitySelector
        )
        {
            this.speed = speed;
            this.bounceCount = bounceCount;
            this.checkEntitySelector = checkEntitySelector;
        }
    }
}