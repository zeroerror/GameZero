#if UNITY_EDITOR
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using UnityEditor;
#endif
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_skill_", menuName = "游戏玩法/配置/技能模板")]
    public class GameSkillSO : GameSOBase
    {
        public string desc;
        public GameSkillType skillType;
        public AnimationClip animClip;
        public string animName;
        public float animLength;
        public GameTimelineEventEM[] timelineEvents;

        public GameSkillConditionEM conditionEM;

        public GameSkillModel ToModel()
        {
            var model = new GameSkillModel(
                typeId,
                skillType,
                animName,
                animLength,
                timelineEvents.ToModels(),
                conditionEM.ToModel());
            return model;
        }

        public GameSkillModelR ToModelR()
        {
            var model = new GameSkillModelR(typeId, skillType, animName, animLength);
            return model;
        }
    }
}
