using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class GameUIJumpTextDomain : GameUIJumpTextDomainApi
    {
        GameUIContext _uiContext;
        private List<GameUIJumpTextEntity> _jumpTextEntityList;
        private Dictionary<string, List<GameUIJumpTextEntity>> _jumpTextEntityPoolDict;

        public GameUIJumpTextDomain()
        {
            this._jumpTextEntityList = new List<GameUIJumpTextEntity>();
            this._jumpTextEntityPoolDict = new Dictionary<string, List<GameUIJumpTextEntity>>();
        }

        public void Inject(GameUIContext uiContext)
        {
            this._uiContext = uiContext;
            this._BindEvent();
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

        private void _BindEvent()
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
                if (!entity.playCom.isPlaying)
                {
                    this._uiContext.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        entity.SetActive(false);
                        this._jumpTextEntityList.Remove(entity);
                        var list = this._jumpTextEntityPoolDict.TryGetValue(entity.prefabName, out var entityList) ? entityList : new List<GameUIJumpTextEntity>();
                        this._jumpTextEntityPoolDict[entity.prefabName] = list;
                        list.Add(entity);
                    });
                }
            }
        }

        private void _PlayAnim(GameUIJumpTextEntity entity, string prefabUrl, int style)
        {
            entity.SetActive(true);

            var playCom = entity.playCom;
            playCom.Stop();
            var prefabName = prefabUrl.Substring(prefabUrl.LastIndexOf('/') + 1);
            var animName = $"{prefabName}_{style}";
            if (playCom.TryGetClip(animName, out var clip))
            {
                playCom.Play(clip);
                return;
            }

            var clipUrl = $"{prefabUrl}_{style}";
            clip = Resources.Load<AnimationClip>(clipUrl);
            if (clip == null)
            {
                GameLogger.LogError($"UI跳字: 加载动画失败 {clipUrl}");
                return;
            }
            playCom.Play(clip);
        }

        public void JumpText_Dmg(in Vector2 screenPos, GameActionDmgType dmgType, int style, string txt, float scale = 1.0f)
        {

            var url = this._GetDmgPrefabUrl(dmgType);
            var prefab = Resources.Load<GameObject>(url);
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
                var txtObj = GameObject.Instantiate(prefab);
                this._uiContext.domainApi.layerApi.AddToUIRoot(txtObj.transform, GameUILayerType.Scene);
                entity = new GameUIJumpTextEntity(txtObj, prefabName);
            }

            entity.text = txt;
            entity.SetAnchorPosition(screenPos);
            this._PlayAnim(entity, url, style);
            this._jumpTextEntityList.Add(entity);
        }

        private string _GetDmgPrefabUrl(GameActionDmgType dmgType)
        {
            var dirName = "";
            switch (dmgType)
            {
                case GameActionDmgType.Real:
                    dirName = "Real";
                    break;
                case GameActionDmgType.Physical:
                    dirName = "Physical";
                    break;
                case GameActionDmgType.Magic:
                    dirName = "Magic";
                    break;
                default:
                    GameLogger.LogError($"UI跳字: 未处理的伤害类型 {dmgType}");
                    break;
            }
            var prefabName = this._GetDmgPrefabName(dmgType);
            var url = $"UI/Battle/JumpText/Dmg/{dirName}/{prefabName}";
            return url;
        }

        private string _GetDmgPrefabName(GameActionDmgType dmgType)
        {
            var suffix = "";
            switch (dmgType)
            {
                case GameActionDmgType.Real:
                    suffix = "real";
                    break;
                case GameActionDmgType.Physical:
                    suffix = "physical";
                    break;
                case GameActionDmgType.Magic:
                    suffix = "magic";
                    break;
                default:
                    GameLogger.LogError($"UI跳字: 未处理的伤害类型 {dmgType}");
                    break;
            }

            return $"jump_text_dmg_{suffix}";
        }
    }
}