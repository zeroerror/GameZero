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
        Neutral = 1 << 2
    }
}