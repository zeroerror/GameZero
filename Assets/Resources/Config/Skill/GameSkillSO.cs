#if UNITY_EDITOR
using System.Runtime.Serialization.Formatters;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Render;
using GamePlay.Core;
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

        public void Save()
        {
            var clip = animClip;
            if (!clip)
            {
                Debug.LogError("动画片段为空");
                return;
            }
            if (animName != clip.name) Debug.Log($"动画名称更新: {animName} => {clip.name}");
            if (animLength != clip.length) Debug.Log($"动画时长更新: {animLength} => {clip.length}");
            // 同步动画名称和时长
            animName = clip.name;
            animLength = clip.length;
            // 同步时间轴事件
            var events = AnimationUtility.GetAnimationEvents(clip);
            var isNew = timelineEvents == null || timelineEvents.Length != events.Length;
            if (isNew)
            {
                timelineEvents = new GameTimelineEventEM[events.Length];
            }
            for (int i = 0; i < timelineEvents.Length; i++)
            {
                var e = events[i];
                var time = e.time;
                var frame = (int)(e.time * GameTimeCollection.frameRate);
                var em = isNew ? new GameTimelineEventEM() : timelineEvents[i];
                if (em.time != time)
                {
                    Debug.Log($"时间轴事件更新[{i + 1}]: {em.time}[{em.frame}] => {time}[{frame}]");
                }
                em.time = time;
                em.frame = frame;
                timelineEvents[i] = em;
            }
            Debug.Log($"保存技能数据 - {typeId} ✔");
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
            var model = new GameSkillModelR(typeId, skillType, animName, animLength);
            return model;
        }
    }
}
