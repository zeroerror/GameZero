using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_projectile_", menuName = "游戏玩法/配置/投射物模板")]
    public class GameProjectileSO : GameSOBase
    {
        // --------------- 渲染数据 ---------------
        public string projectileName;
        public string desc;

        public AnimationClip animClip;

        public GameObject prefab;
        public GameVec2 prefabScale = GameVec2.one;
        public GameVec2 prefabOffset = GameVec2.zero;

        // --------------- 逻辑数据 ---------------
        public float animLength;
        public GameTimelineEventEM[] timelineEvents;
        public float lifeTime;
        // --------------- 编辑器数据 ---------------
        public GameProjectileStateEM[] stateEMs;

        public void UpdateData()
        {
            // 更新动画信息
            if (animClip != null)
            {
                animLength = animClip.length;
                // 读取clip中的事件
                var events = AnimationUtility.GetAnimationEvents(animClip);
                timelineEvents = new GameTimelineEventEM[events.Length];
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
                    timelineEvents[i] = new GameTimelineEventEM
                    {
                        time = e.time,
                        frame = (int)(e.time * GameTimeCollection.frameRate),
                        action = actionSO
                    };
                }
            }
        }


        public GameProjectileModelR ToModelR()
        {
            var prefabUrl = this.prefab?.GetPrefabUrl();
            var model = new GameProjectileModelR(typeId, animClip, prefabUrl, prefabScale, prefabOffset);
            return model;
        }
    }
}
