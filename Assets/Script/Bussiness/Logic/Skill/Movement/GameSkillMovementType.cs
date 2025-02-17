namespace GamePlay.Bussiness.Logic
{
    public enum GameSkillMovementType
    {
        None = 0,
        /// <summary> 闪现 </summary>
        Blink = 1,
        /// <summary> 固定抵达时间的冲刺 </summary>
        FixedTimeDash = 2,
        /// <summary> 固定速度的冲刺 </summary>
        FixedSpeedDash = 3,
    }
}