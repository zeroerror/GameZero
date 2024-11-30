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
        /// 投射物锁定一个固定方向并开始飞行，通常用于箭矢等方向性投射物。
        /// </summary>
        FixedDirection = 2,

        /// <summary>
        /// 投射物锁定某个目标并飞行，朝目标移动。适用于导弹或追踪型投射物。
        /// </summary>
        LockOnEntity = 3,

        /// <summary>
        /// 投射物锁定一个位置并飞行，飞行路径固定，目标位置是静态的。
        /// </summary>
        LockOnPosition = 4,

        /// <summary>
        /// 投射物附着在目标（如角色、地面、障碍物等）上，可能会触发后续效果，如定时炸弹或陷阱。
        /// </summary>
        Attach = 5,

        /// <summary>
        /// 投射物触发爆炸效果，通常表示命中目标或达到预定条件后产生的伤害或其他效果。
        /// </summary>
        Explode = 6,
    }
}