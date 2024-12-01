#if UNITY_EDITOR
using GamePlay.Bussiness.Logic;
using UnityEditor;
#endif
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_skill_", menuName = "游戏玩法/配置/技能模板")]
    public class GameSkillSO : GameSOBase
    {
        [Header("动画文件")]
        public AnimationClip animClip;
        [Header("动画名称")]
        public string animName;
        [Header("动画时长(s)")]
        public float animLength;
        [Header("动画事件")]
        public GameTimelineEventEditModel[] timelineEvents;

        [Header("技能条件")]
        public GameSkillConditionEM conditionEM;

        protected override void OnValidate()
        {
            base.OnValidate();
            // 更新动画信息
            if (animClip != null)
            {
                animName = animClip.name;
                animLength = animClip.length;
                // 读取clip中的事件
                var events = AnimationUtility.GetAnimationEvents(animClip);
                timelineEvents = new GameTimelineEventEditModel[events.Length];
                for (int i = 0; i < events.Length; i++)
                {
                    var e = events[i];
                    var actionId = e.intParameter;
                    // 读取so文件action.asset中的数据，路径为Config/Action
                    var path = $"{GameConfigCollection.ACTION_CONFIG_DIR_PATH}/template_action_{actionId}";
                    var actionSO = Resources.Load<GameActionSO>(path);
                    if (actionSO == null)
                    {
                        if (!EditorUtility.DisplayDialog("行为配置不存在", actionId.ToString(), "了解"))
                        {
                            continue;
                        }
                    }
                    timelineEvents[i] = new GameTimelineEventEditModel
                    {
                        time = e.time,
                        frame = (int)(e.time * GameTimeCollection.frameRate),
                        action = actionSO
                    };
                }
            }
        }

        public GameSkillModel ToModel()
        {
            var model = new GameSkillModel(typeId, animName, animLength, timelineEvents.ToModels(), conditionEM.ToModel());
            return model;
        }
    }
}
