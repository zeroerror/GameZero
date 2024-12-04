namespace GamePlay.Bussiness.Logic
{
    public enum GameProjectileStateType
    {
        None = 0,

        /// <summary>
        /// 待机状态，投射物处于静止不动状态，可能等待被发射或触发其他条件。
        /// </summary>
        Idle = 1,

        /// <summary>
        /// 固定方向飞行，通常用于箭矢等方向性投射物。
        /// </summary>
        FixedDirection = 2,

        /// <summary>
        /// 锁定某个实体目标飞行。适用于导弹或追踪型投射物。
        /// </summary>
        LockOnEntity = 3,

        /// <summary>
        /// 锁定一个地点飞行。飞行路径固定，目标地点是静态的。
        /// </summary>
        LockOnPosition = 4,

        /// <summary>
        /// 附着在目标（如角色、地面、障碍物等）上，可能会触发后续效果，如定时炸弹或陷阱。
        /// </summary>
        Attach = 5,

        /// <summary>
        /// 爆炸，通常表示命中目标或达到预定条件后产生的伤害或其他效果。
        /// </summary>
        Explode = 6,

        /// <summary>
        /// 被销毁，通常表示投射物已经完成了它的使命，或者因为某种原因被强制销毁。
        /// </summary>
        Destroyed = 7,
    }
}