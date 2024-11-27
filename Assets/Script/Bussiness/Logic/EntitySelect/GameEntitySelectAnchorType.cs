namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 实体选择锚点类型, 如单体选择自身或目标，范围选择时的碰撞器坐标系锚点确定
    /// </summary>
    public enum GameEntitySelectAnchorType
    {
        None,
        // 自身
        Self,
        // 目标
        Target,
    }
}