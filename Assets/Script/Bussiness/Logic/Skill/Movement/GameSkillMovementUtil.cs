using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public static class GameSkillMovementUtil
    {

        public static void DoDash(GameSkillEntity skill)
        {
            var movementType = skill.skillModel.movementModel.movementType;
            switch (movementType)
            {
                case GameSkillMovementType.FixedTimeDash:
                    _DoDash_FixedTime(skill);
                    break;
                default:
                    GameLogger.LogError("未知的技能位移类型 " + movementType);
                    break;
            }
        }

        public static void _DoDash_FixedTime(GameSkillEntity skill)
        {
            var movementModel = skill.skillModel.movementModel;
            var dashSpeedModels = movementModel.dashSpeedModels;
            for (int i = 0; i < dashSpeedModels.Length - 1; i++)
            {
                var dash = dashSpeedModels[i];
                var nextDash = dashSpeedModels[i + 1];

                // 正常冲刺
                var isNormalDash = !nextDash.isVariablePoint;
                if (isNormalDash)
                {
                    var speed = dash.speed;
                    var dir = skill.transformCom.forward;
                    var distance = speed * GameTimeCollection.frameTime;
                    var offset = dir * distance;
                    skill.transformCom.position += offset;
                    break;
                }

                // 对目标(即变量点)的冲刺, 在固定时间内冲刺到目标位置
                var curFrame = skill.timelineCom.frame;
                var isStart = dash.frame == curFrame;
                if (isStart)
                {
                    // 记录 开始向目标冲刺的初始位置
                    skill.dashCom.dashToTargetBeginPos = skill.transformCom.position;
                }

                // 根据开始冲刺的位置和目标位置，计算当前帧的位置
                var actionTargeterCom = skill.actionTargeterCom;
                var targetPos = actionTargeterCom.targetEntity?.transformCom.position ?? actionTargeterCom.targetPos;
                if (!targetPos.Equals(GameVec2.zero))
                {
                    var dashFrames = nextDash.frame - dash.frame;
                    var frameOffset = curFrame - dash.frame;
                    var ratio = (float)frameOffset / dashFrames;
                    var dashToTargetBeginPos = skill.dashCom.dashToTargetBeginPos;
                    var curPos = GameMathF.Lerp(dashToTargetBeginPos, targetPos, ratio);
                    skill.transformCom.position = curPos;
                }
            }
        }

    }
}