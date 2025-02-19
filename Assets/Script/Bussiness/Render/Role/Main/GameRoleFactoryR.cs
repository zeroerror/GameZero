using System.Collections.Generic;
using GamePlay.Core;
using GamePlay.Infrastructure;
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
            var tmPrefab = GameResourceManager.Load<GameObject>("Role/Prefab/role");
            var tmRoot = GameObject.Instantiate(tmPrefab);
            tmRoot.transform.localPosition = new Vector3(0, 0, 0);
            var tmBody = tmRoot.transform.Find("body").gameObject;
            var tmFoot = tmRoot.transform.Find("foot").gameObject;

            // 角色预制体
            var rolePrefab = GameResourceManager.Load<GameObject>(model.prefabUrl);
            if (!rolePrefab)
            {
                GameLogger.LogError($"角色工厂[渲染层]: 加载角色预制体失败 {model.prefabUrl}");
                return null;
            }

            var prefabBody = GameObject.Instantiate(rolePrefab);
            prefabBody.transform.SetParent(tmBody.transform);

            // 测试代码, 默认设置弓箭挂点图片
            var attachmentCom = prefabBody.GetComponent<GameRoleAttachmentCom>();
            if (attachmentCom)
            {
                // 加载所有弓箭的精灵
                var bowSprites = GameResourceManager.LoadAll<Sprite>("Equipment/Bow/bow_festive");
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

        public AnimationClip LoadRoleAnimationClip(int typeId, string clipName)
        {
            var url = $"Role/{typeId}/{clipName}";
            var clip = GameResourceManager.LoadAnimationClip(url);
            if (!clip)
            {
                GameLogger.LogError($"角色工厂[渲染层]: 加载动画失败 {url}");
                return null;
            }
            return clip;
        }

        public Slider LoadHPSlider(bool isEnemy)
        {
            var url = isEnemy ? "UI/Battle/attribute_bar_enemy" : "UI/Battle/attribute_bar_ally";
            var prefab = GameResourceManager.Load<GameObject>(url);
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
            var prefab = GameResourceManager.Load<GameObject>(url);
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
            var prefab = GameResourceManager.Load<GameObject>(url);
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

        public Material GetDefaultMaterial()
        {
            if (this._defaultMaterial) return this._defaultMaterial;
            this._defaultMaterial = new Material(Shader.Find("Sprites/Default"));
            return this._defaultMaterial;
        }
        private Material _defaultMaterial;

        /// <summary> 获取分割线材质的实例 </summary>
        public Material CreateSplitLineMaterialInst()
        {
            var mat = GameResourceManager.Load<Material>("UI/Materials/SplitLine/mat_split_line");
            if (!mat)
            {
                GameLogger.LogError("角色工厂[渲染层]: 加载分割线材质失败");
                return null;
            }
            return new Material(mat);
        }
    }
}