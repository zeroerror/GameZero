namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 技能位移模型
    /// <para>冲刺类型存在速度、时间等参数, 闪现类型不存在</para>
    /// <para>冲刺类型固定抵达和固定速度2选1</para>
    /// </summary>
    public class GameSkillMovementModel
    {
        /// <summary> 位移类型 </summary>
        public readonly GameSkillMovementType movementType;
        /// <summary> 冲刺速度模型 </summary>
        public readonly GameSKillDashSpeedModel[] dashSpeedModels;
        /// <summary> 冲刺距离 </summary>
        public readonly float dashDistance;
        /// <summary> 冲刺时暂停技能时间轴 </summary>
        public readonly bool pauseTimeline;

        public GameSkillMovementModel(
            GameSkillMovementType movementType,
            GameSKillDashSpeedModel[] dashSpeedModels,
            float dashDistance,
            bool pauseTimeline
        )
        {
            this.movementType = movementType;
            this.dashSpeedModels = dashSpeedModels;
            this.dashDistance = dashDistance;
            this.pauseTimeline = pauseTimeline;
        }
    }
}