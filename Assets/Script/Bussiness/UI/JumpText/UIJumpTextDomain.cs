using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class UIJumpTextDomain : UIJumpTextDomainApi
    {
        UIContext _uiContext;
        private List<UIJumpTextEntity> _jumpTextEntityList;
        private Dictionary<string, List<UIJumpTextEntity>> _jumpTextEntityPoolDict;

        public UIJumpTextDomain()
        {
            this._jumpTextEntityList = new List<UIJumpTextEntity>();
            this._jumpTextEntityPoolDict = new Dictionary<string, List<UIJumpTextEntity>>();
        }

        public void Inject(UIContext uiContext)
        {
            this._uiContext = uiContext;
            this._BindEvents();
        }

        public void Destroy()
        {
            this._UnbindEvents();
            this._jumpTextEntityList?.Foreach((entity) =>
            {
                entity.Destroy();
            });
            this._jumpTextEntityList.Clear();

            foreach (var pair in this._jumpTextEntityPoolDict)
            {
                pair.Value.Foreach((entity) =>
                {
                    entity.Destroy();
                });
                pair.Value.Clear();
            }
            this._jumpTextEntityPoolDict.Clear();
        }

        private void _BindEvents()
        {
        }

        private void _UnbindEvents()
        {
        }

        public void Tick(float dt)
        {
            for (var i = 0; i < this._jumpTextEntityList.Count; i++)
            {
                var entity = this._jumpTextEntityList[i];
                entity.Tick(dt);
                if (!entity.animCom.isPlaying)
                {
                    this._uiContext.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        entity.SetActive(false);
                        this._jumpTextEntityList.Remove(entity);
                        var list = this._jumpTextEntityPoolDict.TryGetValue(entity.prefabName, out var entityList) ? entityList : new List<UIJumpTextEntity>();
                        this._jumpTextEntityPoolDict[entity.prefabName] = list;
                        list.Add(entity);
                    });
                }
            }
        }

        private void _PlayAnim(UIJumpTextEntity entity, string prefabUrl, int style)
        {
            entity.SetActive(true);

            var animCom = entity.animCom;
            animCom.Stop();
            var prefabName = prefabUrl.Substring(prefabUrl.LastIndexOf('/') + 1);
            var animName = $"{prefabName}_{style}";
            if (animCom.TryGetClip(animName, out var clip))
            {
                animCom.Play(clip);
                return;
            }

            var clipUrl = $"{prefabUrl}_{style}";
            clip = GameResourceManager.LoadAnimationClip(clipUrl);
            if (clip == null)
            {
                GameLogger.LogError($"UI跳字: 加载动画失败 {clipUrl}");
                return;
            }
            animCom.Play(clip);
        }

        public void JumpText_Dmg(in Vector2 screenPos, GameActionDmgType dmgType, bool isCrit, int style, string txt, float scale = 1.0f)
        {

            var url = this._GetDmgPrefabUrl(dmgType, isCrit);
            var prefab = GameResourceManager.Load<GameObject>(url);
            if (!prefab)
            {
                GameLogger.LogError($"UI跳字: 加载跳字失败 {url}");
                return;
            }

            var prefabName = prefab.name;
            var entity = this._jumpTextEntityPoolDict.TryGetValue(prefabName, out var entityList) ? entityList.Count > 0 ? entityList[0] : null : null;
            if (entity != null)
            {
                entityList.Remove(entity);
            }
            else
            {
                var txtGO = GameObject.Instantiate(prefab);
                var rootGO = new GameObject(prefabName, typeof(RectTransform));
                txtGO.transform.SetParent(rootGO.transform, false);
                this._uiContext.uiApi.layerApi.AddToUIRoot(rootGO.transform, UILayerType.Scene);
                entity = new UIJumpTextEntity(rootGO, txtGO, prefabName);
            }

            entity.text = txt;
            entity.SetAnchorPosition(screenPos);
            entity.SetScale(scale);
            this._PlayAnim(entity, url, style);
            this._jumpTextEntityList.Add(entity);
        }

        private string _GetDmgPrefabUrl(GameActionDmgType dmgType, bool isCrit)
        {
            string dirName;
            switch (dmgType)
            {
                case GameActionDmgType.Real:
                    dirName = "Real";
                    break;
                case GameActionDmgType.Normal:
                    dirName = "Normal";
                    break;
                default:
                    GameLogger.LogError($"UI跳字: 未处理的伤害类型 {dmgType}");
                    return string.Empty;
            }
            var prefabName = this._GetDmgPrefabName(dmgType, isCrit);
            var url = $"UI/Battle/JumpText/Dmg/{dirName}/{prefabName}";
            return url;
        }

        private string _GetDmgPrefabName(GameActionDmgType dmgType, bool isCrit)
        {
            var suffix = "";
            switch (dmgType)
            {
                case GameActionDmgType.Real:
                    suffix = "real";
                    break;
                case GameActionDmgType.Normal:
                    suffix = "normal";
                    break;
                default:
                    GameLogger.LogError($"UI跳字: 未处理的伤害类型 {dmgType}");
                    break;
            }
            if (isCrit)
            {
                suffix += "_crit";
            }

            return $"jump_text_dmg_{suffix}";
        }
    }
}