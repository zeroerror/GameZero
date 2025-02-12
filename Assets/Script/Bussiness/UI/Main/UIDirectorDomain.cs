using System;
using GamePlay.Bussiness.Core;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Render;
using GamePlay.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Bussiness.UI
{
    public class UIDirectorDomain : UIDirectorDomainApi
    {
        public UIContext context { get; private set; }
        public UIDebugDomain debugDomain { get; private set; }
        public UIJumpTextDomain jumpTextDomain { get; private set; }
        public UILayerDomain layerDomain { get; private set; }

        #region [系统]

        public UIPlayerDomain playerDomain { get; private set; }
        public UIActionOptionDomain actionOptionDomain { get; private set; }
        public UISettlingDomain settlingDomain { get; private set; }
        public UIUnitShopDomain unitShopDomain { get; private set; }

        #endregion

        public UIDirectorDomain()
        {
            this.context = new UIContext();
            this._InitDomain();
        }

        private void _InitDomain()
        {
            this.debugDomain = new UIDebugDomain();
            this.jumpTextDomain = new UIJumpTextDomain();
            this.layerDomain = new UILayerDomain();
            this.playerDomain = new UIPlayerDomain();
            this.actionOptionDomain = new UIActionOptionDomain();
            this.settlingDomain = new UISettlingDomain();
            this.unitShopDomain = new UIUnitShopDomain();
        }

        public void Inject(GameObject uiRoot, GameDomainApi logicApi, GameDomainApiR rendererApi)
        {
            this._InitContext(uiRoot, logicApi, rendererApi);
            this._InjectContext();
        }

        private void _InitContext(GameObject uiRoot, GameDomainApi logicApi, GameDomainApiR rendererApi)
        {
            this.context.Inject(uiRoot, logicApi, rendererApi);
            var domainApi = this.context.uiApi;
            domainApi.InjectApis(
                logicApi,
                rendererApi,
                this,
                this.jumpTextDomain,
                this.layerDomain,
                this.playerDomain,
                this.actionOptionDomain,
                this.settlingDomain,
                this.unitShopDomain
            );
        }

        private void _InjectContext()
        {
            this.debugDomain.Inject(this.context);
            this.jumpTextDomain.Inject(this.context);
            this.layerDomain.Inject(this.context);
            this.playerDomain.Inject(this.context);
            this.actionOptionDomain.Inject(this.context);
            this.settlingDomain.Inject(this.context);
            this.unitShopDomain.Inject(this.context);
        }

        public void Destroy()
        {
            this.debugDomain.Destroy();
            this.jumpTextDomain.Destroy();
            this.layerDomain.Destroy();
            this.playerDomain.Destroy();
            this.actionOptionDomain.Destroy();
            this.settlingDomain.Destroy();
            this.unitShopDomain.Destroy();
        }

        protected void _TickDomain(float dt)
        {
            this.debugDomain.Tick();
            this.jumpTextDomain.Tick(dt);
            this.layerDomain.Tick(dt);
            this.playerDomain.Tick(dt);
            this.actionOptionDomain.Tick(dt);
            this.settlingDomain.Tick(dt);
            this.unitShopDomain.Tick(dt);
        }

        public void Update(float dt)
        {
            this.context.delayRCEventService.Tick();
            this.context.eventService.Tick();
            this.context.inputService.Tick();

            var director = this.context.director;
            director.Tick(dt);
            dt *= director.timeScaleCom.timeScale;
            this._Tick(dt);
        }

        public void LateUpdate(float dt)
        {
            this._LateTick(dt);
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

        public UIBase OpenUI<T>(UIViewInput viewInput = null) where T : UIBase
        {
            var inst = Activator.CreateInstance<T>();
            var uiName = inst.uiName;
            if (this.context.uiDict.TryGetValue(uiName, out var uiBase))
            {
                GameLogger.DebugLog($"UI刷新: {uiName}");
                this._ExecuteUILifeCycle(uiBase, uiBase.go, viewInput);
                return uiBase;
            }

            // 加载UI资源
            uiBase = inst;
            var uiUrl = uiBase.uiUrl;
            var uiGO = this.context.factory.LoadUI(uiUrl);
            if (uiGO == null) return null;

            // 设置对应层级
            var layerType = uiBase.layerType;
            var layer = this.context.layerDict[layerType];
            uiGO.transform.SetParent(layer.transform, false);
            // 根据分辨率设置rootgo的recttransform
            uiGO.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

            // window 默认添加一个灰色背景
            if (uiBase.layerType == UILayerType.Window)
            {
                var maskImage = uiGO.GetComponent<Image>() ?? uiGO.AddComponent<Image>();
                maskImage.color = new Color(0, 0, 0, 0.5f);
                maskImage.raycastTarget = true;
                // 点击背景关闭
                uiBase.SetClick(maskImage.gameObject, () => this.CloseUI(uiName));
            }

            this._ExecuteUILifeCycle(uiBase, uiGO, viewInput);
            this.context.uiDict[uiName] = uiBase;
            GameLogger.DebugLog($"UI打开: {uiName}");
            return uiBase;
        }

        /// <summary> UI生命周期 </summary>
        private void _ExecuteUILifeCycle(UIBase uiBase, GameObject uiGO, UIViewInput viewInput)
        {
            uiBase.Inject(uiGO, this.context.uiApi);
            uiBase.Init(viewInput);
            uiBase.Show();
        }

        public void CloseUI(string uiName)
        {
            if (!this.context.uiDict.TryGetValue(uiName, out var ui))
            {
                GameLogger.LogWarning($"UI不存在或已经关闭: {uiName}");
                return;
            }
            ui.Destroy();
            this.context.uiDict.Remove(uiName);
            GameLogger.DebugLog($"UI关闭: {uiName}");
        }

        public void CloseUI<T>() where T : UIBase
        {
            var inst = Activator.CreateInstance<T>();
            this.CloseUI(inst.uiName);
        }

        public void CloseUI<T>(T ui) where T : UIBase
        {
            this.CloseUI(ui.uiName);
        }

        public void BindKeyAction(KeyCode keyCode, Action callback, GameInputStateType stateType)
        {
            this.context.inputService.BindKeyAction(keyCode, callback, stateType);
        }

        public void UnbindKeyAction(KeyCode keyCode, Action callback, GameInputStateType stateType)
        {
            this.context.inputService.UnbindKeyAction(keyCode, callback, stateType);
        }

        public void BindEvent(string eventName, Action<object> callback)
        {
            this.context.eventService.Bind(eventName, callback);
        }

        public void UnbindEvent(string eventName, Action<object> callback)
        {
            this.context.eventService.Unbind(eventName, callback);
        }

        public Vector3 GetPointerPosition()
        {
            var pointerPos = Input.mousePosition;
            return pointerPos;
        }
    }
}