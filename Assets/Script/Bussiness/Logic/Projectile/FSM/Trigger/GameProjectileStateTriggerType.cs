namespace GamePlay.Bussiness.Logic
{
    public enum GameProjectileStateTriggerType
    {
        None = 0,
        // 持续时间
        Duration = 1,
        // 发生体积碰撞
        VolumeCollision = 2,
        // 抵达目标, 包括实体目标和位置目标
        ArriveTarget = 3,
    }
}