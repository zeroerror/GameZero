using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameProjectileStateModel_LockOnEntity
    {
        /// <summary> 目标选取类型 </summary>
        public GameProjectileTargeterType targeterType;
        /// <summary> 速度 </summary>
        public float speed;
        /// <summary> 一定时间抵达. 生效时速度参数失效 </summary>
        public float timeToArrive;

        public GameProjectileStateModel_LockOnEntity(GameProjectileTargeterType targeterType, float speed, float duration)
        {
            this.targeterType = targeterType;
            this.speed = speed;
            this.timeToArrive = duration;
        }
    }
}