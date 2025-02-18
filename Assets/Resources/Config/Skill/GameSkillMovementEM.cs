using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameSkillMovementEM
    {
        public GameSkillMovementType movementType;
        public bool pauseTimeline;

        // 工具会根据动画(规范前提下), 提取冲刺的时间、冲刺的固定抵达时间或固定速度
        public AnimationClip clip;
        public GameSKillDashEM[] speedEMs;
        public float dashDistance;

        public GameSkillMovementModel ToModel()
        {
            return new GameSkillMovementModel(
                movementType,
                movementType != GameSkillMovementType.None ? speedEMs?.ToModels() : null,
                dashDistance,
                pauseTimeline
            );
        }
    }
}