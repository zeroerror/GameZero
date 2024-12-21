using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class GameUIJumpTextDomain : GameUIJumpTextDomainApi
    {
        GameUIContext _uiContext;
        private List<GameUIJumpTextEntity> _jumpTextEntityList;
        private List<GameUIJumpTextEntity> _jumpTextEntityPool;

        public GameUIJumpTextDomain()
        {
            this._jumpTextEntityList = new List<GameUIJumpTextEntity>();
            this._jumpTextEntityPool = new List<GameUIJumpTextEntity>();
        }

        public void Inject(GameUIContext uiContext)
        {
            this._uiContext = uiContext;
            this._BindEvent();
        }

        public void Destroy()
        {
            this._UnbindEvents();
            this._jumpTextEntityList?.ForEach((entity) =>
            {
                entity.Destroy();
            });
            this._jumpTextEntityList.Clear();
            this._jumpTextEntityPool?.ForEach((entity) =>
            {
                entity.Destroy();
            });
            this._jumpTextEntityPool.Clear();
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
                if (!entity.playCom.IsPlaying)
                {
                    this._uiContext.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        entity.SetActive(false);
                        this._jumpTextEntityList.Remove(entity);
                        this._jumpTextEntityPool.Add(entity);
                    });
                }
            }
        }

        public void JumpText_Dmg(in Vector2 screenPos, int style, string txt, float scale = 1.0f)
        {
            var prefabName = $"jump_text_{GameUIJumpTextType.Dmg.ToText()}";
            var url = $"UI/Battle/{prefabName}";
            var prefab = Resources.Load<GameObject>(url);
            if (!prefab)
            {
                GameLogger.LogError($"UI跳字: 加载跳字失败 {url}");
                return;
            }

            var entity = this._jumpTextEntityPool.Count > 0 ? this._jumpTextEntityPool[this._jumpTextEntityPool.Count - 1] : null;
            if (entity != null)
            {
                this._jumpTextEntityPool.RemoveAt(this._jumpTextEntityPool.Count - 1);
            }
            else
            {
                var txtObj = GameObject.Instantiate(prefab);
                this._uiContext.AddToUIRoot(txtObj.transform);
                entity = new GameUIJumpTextEntity(txtObj);
            }

            entity.text = txt;
            entity.SetAnchorPosition(screenPos);
            this.PlayAnim(entity, prefabName, style);
            this._jumpTextEntityList.Add(entity);
        }

        public void PlayAnim(GameUIJumpTextEntity entity, string prefabName, int style)
        {
            entity.SetActive(true);

            var playCom = entity.playCom;
            var animName = $"{prefabName}_{style}";
            if (playCom.hasClip(animName))
            {
                playCom.Play(animName);
                return;
            }

            var clipUrl = $"UI/Battle/{animName}";
            var clip = Resources.Load<AnimationClip>(clipUrl);
            if (clip == null)
            {
                GameLogger.LogError($"UI跳字: 加载动画失败 {clipUrl}");
                return;
            }
            playCom.Play(clip);
        }
    }
}