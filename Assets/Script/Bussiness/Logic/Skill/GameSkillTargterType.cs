namespace GamePlay.Bussiness.Logic
{
    public enum GameSkillTargterType
    {
        None,
        /// <summary>
        /// 选中的敌人
        /// </summary>
        Enemy = 1,
        /// <summary>
        /// 选中的方向
        /// </summary>
        Direction = 2,
        /// <summary>
        /// 选中的位置
        /// </summary>
        Position = 3,
        /// <summary>
        /// 自己
        /// </summary>
        Self = 4,
    }
}