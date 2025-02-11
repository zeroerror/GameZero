using GamePlay.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Bussiness.Render
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
            var bodyCom = GetBodyCom(model);
            var e = new GameRoleEntityR(model, bodyCom);
            return e;
        }

        public GameRoleBodyCom GetBodyCom(GameRoleModelR model)
        {
            // 模板预制体
            var tmPrefab = Resources.Load<GameObject>("Role/Prefab/role");
            var tmRoot = GameObject.Instantiate(tmPrefab);
            tmRoot.transform.localPosition = new Vector3(0, 0, 0);
            var tmBody = tmRoot.transform.Find("body").gameObject;
            var tmFoot = tmRoot.transform.Find("foot").gameObject;

            // 角色预制体
            var rolePrefab = Resources.Load<GameObject>(model.prefabUrl);
            if (!rolePrefab)
            {
                GameLogger.LogError($"角色工厂[渲染层]: 加载角色预制体失败 {model.prefabUrl}");
                return null;
            }
            var prefabBody = GameObject.Instantiate(rolePrefab);
            if (!prefabBody.TryGetComponent<Animator>(out var animator)) animator = prefabBody.AddComponent<Animator>();
            if (!prefabBody.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) spriteRenderer = prefabBody.AddComponent<SpriteRenderer>();
            prefabBody.transform.SetParent(tmBody.transform);

            // 角色挂点
            var attachmentCom = prefabBody.GetComponent<GameRoleAttachmentCom>();
            // 测试代码, 默认设置弓箭挂点图片
            if (attachmentCom)
            {
                // 加载所有弓箭的精灵
                var bowSprites = Resources.LoadAll<Sprite>("Equipment/Bow/bow_festive");
                var bowHandleSprite = bowSprites.Find(sprite => sprite.name == "bow_handle_festive");
                var bowLimbSprite = bowSprites.Find(sprite => sprite.name == "bow_limb_festive");
                GameLogger.Assert(bowHandleSprite != null, "角色工厂[渲染层]: 加载弓箭挂点图片失败");
                GameLogger.Assert(bowLimbSprite != null, "角色工厂[渲染层]: 加载弓箭挂点图片失败");
                attachmentCom.SetAttachmentSprite_Bow(bowHandleSprite, bowLimbSprite);
                attachmentCom.SetAttachmentSprite_Visible(GameRoleAttachmentDirectionType.Left, true);
            }
            var bodyCom = new GameRoleBodyCom(tmRoot, tmFoot, prefabBody, attachmentCom);
            return bodyCom;
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
            var url = isEnemy ? "UI/Battle/attribute_bar_enemy" : "UI/Battle/attribute_bar_ally";
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

        public Slider LoadShieldSlider()
        {
            var url = "UI/Battle/attribute_bar_shield";
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
            var url = "UI/Battle/attribute_bar_mp";
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
    }
}