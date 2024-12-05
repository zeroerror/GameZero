namespace GamePlay.Bussiness.Logic
{
    public struct GameProjectileStateModel_LockOnPosition
    {
        /// <summary> 目标选取类型 </summary>
        public readonly GameProjectileTargeterType targeterType;
        /// <summary> 速度 </summary>
        public readonly float speed;
        /// <summary> 抵达目标时间, 设置时速度参数失效 </summary>
        public readonly float duration;

        public GameProjectileStateModel_LockOnPosition(GameProjectileTargeterType targeterType, float speed, float duration)
        {
            this.targeterType = targeterType;
            this.speed = speed;
            this.duration = duration;
        }
    }
}