#if UNITY_EDITOR
using GamePlay.Bussiness.Logic;
using UnityEditor;
#endif
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_projectile_", menuName = "游戏玩法/配置/投射物模板")]
    public class GameProjectileSO : GameSOBase
    {
        // --------------- 渲染数据 ---------------
        [Header("名称")]
        public string projectileName;
        [Header("描述")]
        public string desc;
        [Header("动画文件")]
        public AnimationClip animClip;

        // --------------- 逻辑数据 ---------------
        [Header("动画时长(s)")]
        public float animLength;
        [Header("动画事件")]
        public GameTimelineEventEditModel[] timelineEvents;
        [Header("生命周期(s)")]
        public float lifeTime;
        // --------------- 编辑器数据 ---------------
        [Header("动画名称")]
        public string animName_edit;
        [Header("状态模型列表")]
        public GameProjectileStateEM[] stateEMs;

        protected override void OnValidate()
        {
            base.OnValidate();
            // 更新动画信息
            if (animClip != null)
            {
                animName_edit = animClip.name;
                animLength = animClip.length;
                // 读取clip中的事件
                var events = AnimationUtility.GetAnimationEvents(animClip);
                timelineEvents = new GameTimelineEventEditModel[events.Length];
                for (int i = 0; i < events.Length; i++)
                {
                    var e = events[i];
                    var actionId = e.intParameter;
                    var prefix = this._getPrefix();
                    var path = $"{GameConfigCollection.ACTION_CONFIG_DIR_PATH}/{prefix}{actionId}";
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
    }
}
