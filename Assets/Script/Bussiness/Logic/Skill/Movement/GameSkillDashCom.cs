using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameSkillDashCom
    {
        /// <summary> 开始向目标冲刺的初始位置 </summary>
        public GameVec2 dashToTargetBeginPos;
        /// <summary> 冲刺开始帧 </summary>
        public readonly float dashBeginFrame;
        /// <summary> 冲刺结束帧 </summary>
        public readonly float dashEndFrame;
        /// <summary> 是否需要暂停时间轴 </summary>
        public readonly bool pauseTimeline;

        public GameSkillDashCom(GameSkillMovementModel movementModel)
        {
            var dashModels = movementModel.dashModels;
            if (dashModels != null && dashModels.Length > 0)
            {
                this.dashBeginFrame = dashModels[0].frame;
                this.dashEndFrame = dashModels[dashModels.Length - 1].frame;
            }
            this.pauseTimeline = movementModel.pauseTimeline;
        }

        public void Clear()
        {
            this.dashToTargetBeginPos = GameVec2.zero;
        }
    }
}