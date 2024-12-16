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
            var prefab = Resources.Load<GameObject>("Role/Prefab/role");
            var go = GameObject.Instantiate(prefab);
            var body = go.transform.Find("body").gameObject;
            var foot = go.transform.Find("foot").gameObject;

            var typePrefab = Resources.Load<GameObject>($"Role/{typeId}/role_prefab_{typeId}");
            var roleGO = typePrefab ? GameObject.Instantiate(typePrefab) : new GameObject();
            roleGO.AddComponent<Animator>().runtimeAnimatorController = null;
            roleGO.AddComponent<SpriteRenderer>();
            roleGO.transform.SetParent(body.transform);

            var scale = body.transform.localScale;
            scale.x = -scale.x;
            body.transform.localScale = scale;

            go.transform.localPosition = new Vector3(0, 0, 0);
            var e = new GameRoleEntityR(typeId, go, foot, body);
            return e;
        }

        public AnimationClip LoadAnimationClip(int typeId, string clipName)
        {
            var res = Resources.Load<AnimationClip>($"Role/{typeId}/Anim/{clipName}");
            if (!res)
            {
                GameLogger.LogError($"角色工厂[渲染层]: 加载动画失败 {typeId} {clipName}");
            }
            return res;
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