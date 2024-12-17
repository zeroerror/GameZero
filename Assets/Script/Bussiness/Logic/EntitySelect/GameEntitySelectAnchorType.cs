namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 实体选择锚点类型, 如单体选择自身或目标，范围选择时的碰撞器坐标系锚点确定
    /// </summary>
    public enum GameEntitySelectAnchorType
    {
        None,
        /// <summary> 行为者 </summary>
        Actor,
        /// <summary> 行为目标 </summary>
        ActTarget,
        /// <summary> 行为者角色 </summary>
        ActorRole,
        /// <summary> 最近的敌人 </summary>
        NearestEnemy,
    }

    public static class GameEntitySelectAnchorTypeExtensions
    {
        public static string ToString(this GameEntitySelectAnchorType type)
        {
            switch (type)
            {
                case GameEntitySelectAnchorType.Actor:
                    return "行为者";
                case GameEntitySelectAnchorType.ActTarget:
                    return "行为目标";
                case GameEntitySelectAnchorType.ActorRole:
                    return "行为者角色";
                case GameEntitySelectAnchorType.NearestEnemy:
                    return "最近的敌人";
                case GameEntitySelectAnchorType.None:
                default:
                    return "无";
            }
        }
    }
}
