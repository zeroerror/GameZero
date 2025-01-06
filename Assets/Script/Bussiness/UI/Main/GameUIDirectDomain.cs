using System;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Bussiness.UI
{
    public class GameUIDirectDomain : GameUIDirectDomainApi
    {
        public GameUIContext context { get; private set; }
        public GameUIDebugDomain debugDomain { get; private set; }
        public GameUIJumpTextDomain jumpTextDomain { get; private set; }
        public GameUILayerDomain layerDomain { get; private set; }

        public GameUIDirectDomain()
        {
            this.context = new GameUIContext();
            this._InitDomain();
            this.SetTimeout(1, () =>
            {
                this.OpenUI<GameUI_ActionOption>();
            });
        }

        private void _InitDomain()
        {
            this.debugDomain = new GameUIDebugDomain();
            this.jumpTextDomain = new GameUIJumpTextDomain();
            this.layerDomain = new GameUILayerDomain();
        }

        public void Inject(GameObject uiRoot, GameDomainApi logicApi, GameDomainApiR rendererApi)
        {
            this._InitContext(uiRoot, logicApi, rendererApi);
            this._InjectContext();
        }

        private void _InitContext(GameObject uiRoot, GameDomainApi logicApi, GameDomainApiR rendererApi)
        {
            this.context.Inject(uiRoot, logicApi, rendererApi);
            var domainApi = this.context.domainApi;
            domainApi.SetDirectDomainApi(this);
            domainApi.SetJumpTextDomainApi(this.jumpTextDomain);
            domainApi.SetLayerApi(this.layerDomain);
            domainApi.SetRendererApi(rendererApi);
        }

        private void _InjectContext()
        {
            this.debugDomain.Inject(this.context);
            this.jumpTextDomain.Inject(this.context);
            this.layerDomain.Inject(this.context);
        }

        public void Destroy()
        {
            this.debugDomain.Destroy();
            this.jumpTextDomain.Destroy();
            this.layerDomain.Destroy();
        }

        protected void _TickDomain(float dt)
        {
            this.debugDomain.Tick();
            this.jumpTextDomain.Tick(dt);
            this.layerDomain.Tick(dt);
        }

        public void Update(float dt)
        {
            var director = this.context.director;
            director.Tick(dt);
            dt *= director.timeScaleCom.timeScale;
            this._PreTick(dt);
            this._Tick(dt);
        }

        public void LateUpdate(float dt)
        {
            this._LateTick(dt);
        }

        protected void _PreTick(float dt)
        {
        }

        protected void _Tick(float dt)
        {
            this._TickDomain(dt);
        }

        protected void _LateTick(float dt)
        {
            this.context.cmdBufferService.Tick();
        }

        public int SetInterval(float interval, Action callback)
        {
            return this.context.cmdBufferService.AddIntervalCmd(interval, callback);
        }

        public int SetTimeout(float delay, Action callback)
        {
            return this.context.cmdBufferService.AddDelayCmd(delay, callback);
        }

        public void RemoveTimer(int timerId)
        {
            this.context.cmdBufferService.Remove(timerId);
        }

        public void OpenUI<T>(object args = null) where T : GameUIBase
        {
            var uiName = typeof(T).Name;
            if (this.context.uiBaseDict.TryGetValue(uiName, out var uiBase))
            {
                uiBase.Show();
                return;
            }

            // 创建UI实例
            uiBase = Activator.CreateInstance<T>();

            // 加载UI资源
            var uiUrl = uiBase.uiUrl;
            var rootGO = this.context.factory.LoadUI(uiUrl);
            if (rootGO == null) return;

            // 设置对应层级
            var layerType = uiBase.layerType;
            var layer = this.context.layerDict[layerType];
            rootGO.transform.SetParent(layer.transform, false);
            // 根据分辨率设置rootgo的recttransform
            rootGO.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

            // 弹窗、window 默认添加一个灰色背景
            if (uiBase.layerType == GameUILayerType.PopUp || uiBase.layerType == GameUILayerType.Window)
            {
                var maskImage = rootGO.GetComponent<Image>() ?? rootGO.AddComponent<Image>();
                maskImage.color = new Color(0, 0, 0, 0.5f);
                maskImage.raycastTarget = true;
            }

            // UI生命周期
            uiBase.Inject(rootGO, this.context.domainApi);
            uiBase.Init(args);
            uiBase.Show();
            this.context.uiBaseDict[uiName] = uiBase;
        }
    }
}