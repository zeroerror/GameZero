using GamePlay.Core;
using GamePlay.Infrastructure;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public static class GameSkillDashUtil
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
                    GameLogger.LogError("GameSkillDashUtil.DoDash: movementType not support: " + movementType);
                    break;
            }
        }

        public static void _DoDash_FixedTime(GameSkillEntity skill)
        {
            var movementModel = skill.skillModel.movementModel;
            var dashModels = movementModel.dashModels;
            var curFrame = skill.timelineCom.frame;

            // 记录 开始向目标冲刺的初始位置
            var isStart = skill.dashCom.dashBeginFrame == curFrame;
            if (isStart)
            {
                skill.dashCom.dashToTargetBeginPos = skill.transformCom.position;
            }

            for (int i = 0; i < dashModels.Length - 1; i++)
            {
                var dash = dashModels[i];
                var nextDash = dashModels[i + 1];
                var isCurDash = dash.frame <= curFrame && curFrame < nextDash.frame;
                if (!isCurDash)
                {
                    continue;
                }

                // 根据开始冲刺的位置和目标位置，计算当前帧的位置
                // 确定目标位置
                GameVec2 targetPos;
                var curDisRatio = dash.distanceRatio;
                var toDisRatio = nextDash.distanceRatio;
                var actionTargeterCom = skill.actionTargeterCom;
                var targetEntity = actionTargeterCom.targetEntity;
                var dashToTargetBeginPos = skill.dashCom.dashToTargetBeginPos;
                if (targetEntity != null)
                {
                    // 对于实体目标, 需要减少一个碰撞半径的距离
                    targetPos = targetEntity.logicBottomPos;
                    var dir = (dashToTargetBeginPos - targetPos).normalized;
                    targetPos -= GameRoleCollection.ROLE_COLLIDER_RADIUS * dir;
                }
                else
                {
                    targetPos = actionTargeterCom.targetPos;
                }
                // 存在目标位置时，计算当前冲刺到的位置
                if (!targetPos.Equals(GameVec2.zero))
                {
                    var dashFrames = nextDash.frame - dash.frame;
                    var frameOffset = curFrame - dash.frame;
                    var timeRatio = (float)frameOffset / dashFrames;
                    var ratio = curDisRatio + (toDisRatio - curDisRatio) * timeRatio;
                    var curPos = GameMathF.Lerp(dashToTargetBeginPos, targetPos, ratio);
                    skill.transformCom.position = curPos;
                }
                break;
            }
        }

    }
}