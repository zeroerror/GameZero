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
        [Header("描述")]
        public string desc;
        [Header("技能类型")]
        public GameSkillType skillType;
        [Header("动画文件")]
        public AnimationClip animClip;
        [Header("动画名称")]
        public string animName;
        [Header("动画时长(s)")]
        public float animLength;
        [Header("动画事件")]
        public GameTimelineEventEM[] timelineEvents;

        [Header("技能条件")]
        public GameSkillConditionEM conditionEM;

        public void Update()
        {
            if (animClip != null)
            {
                animName = animClip.name;
                animLength = animClip.length;
                var events = AnimationUtility.GetAnimationEvents(animClip);
                if (timelineEvents == null || timelineEvents.Length != events.Length) timelineEvents = new GameTimelineEventEM[events.Length];
                for (int i = 0; i < events.Length; i++)
                {
                    var e = events[i];
                    timelineEvents[i].time = e.time;
                    timelineEvents[i].frame = (int)(e.time * GameTimeCollection.frameRate);
                }
            }
        }

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
            var model = new GameSkillModelR(typeId, skillType, animClip);
            return model;
        }
    }
}
