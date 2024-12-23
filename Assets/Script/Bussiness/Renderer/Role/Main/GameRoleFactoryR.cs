using GamePlay.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleFactoryR
    {

        public GameRoleTemplateR template { get; private set; }

        public GameRoleFactoryR()
        {
            this.template = new GameRoleTemplateR();
        }

        public GameRoleEntityR Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("角色创建失败，角色ID不存在：" + typeId);
                return null;
            }

            var prefab = Resources.Load<GameObject>("Role/Prefab/role");
            var go = GameObject.Instantiate(prefab);
            var body = go.transform.Find("body").gameObject;
            var foot = go.transform.Find("foot").gameObject;

            var typePrefab = Resources.Load<GameObject>(model.prefabUrl);
            var roleGO = typePrefab ? GameObject.Instantiate(typePrefab) : new GameObject();
            if (!roleGO.TryGetComponent<Animator>(out var animator)) animator = roleGO.AddComponent<Animator>();
            if (!roleGO.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) spriteRenderer = roleGO.AddComponent<SpriteRenderer>();
            roleGO.transform.SetParent(body.transform);

            var scale = body.transform.localScale;
            body.transform.localScale = scale;

            go.transform.localPosition = new Vector3(0, 0, 0);
            var e = new GameRoleEntityR(model, go, foot, body);
            return e;
        }

        public AnimationClip LoadAnimationClip(int typeId, string clipName)
        {
            var animClip = Resources.Load<AnimationClip>($"Role/{typeId}/{clipName}");
            if (!animClip)
            {
                GameLogger.LogError($"角色工厂[渲染层]: 加载动画失败 {typeId} {clipName}");
            }
            animClip.events = null;// 动画事件忽略
            return animClip;
        }

        public Slider LoadHPSlider(bool isEnemy)
        {
            var url = isEnemy ? "UI/Battle/attribute_bar_enemy" : "UI/Battle/attribute_bar_self";
            var prefab = Resources.Load<GameObject>(url);
            if (!prefab)
            {
                GameLogger.LogError($"角色工厂[渲染层]: 加载属性条失败 {url}");
                return null;
            }
            var go = GameObject.Instantiate(prefab);
            var slider = go.GetComponentInChildren<Slider>();
            Debug.Assert(slider != null, "角色工厂[渲染层]: 加载属性条失败, 未找到Slider组件");
            return slider;
        }

        public Slider LoadMPSlider()
        {
            var slider = this.LoadHPSlider(false);
            return slider;
        }
    }
}