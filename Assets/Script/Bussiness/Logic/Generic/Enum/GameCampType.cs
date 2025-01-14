namespace GamePlay.Bussiness.Logic
{
    public enum GameCampType
    {
        None = 0,
        // 敌方
        Enemy = 1 << 0,
        // 友方
        Ally = 1 << 1,
        // 中立
        Neutral = 1 << 2,
        // 所有
        All = Enemy | Ally | Neutral
    }

    public static class GameCampTypeExt
    {
        public static string ToDesc(this GameCampType campType)
        {
            switch (campType)
            {
                case GameCampType.None:
                    return "无";
                case GameCampType.Enemy:
                    return "敌方";
                case GameCampType.Ally:
                    return "友方";
                case GameCampType.Neutral:
                    return "中立";
                case GameCampType.All:
                    return "所有";
                default:
                    return "未知";
            }
        }
    }
}