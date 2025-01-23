using System;
using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public abstract class UIBase
    {
        public GameObject go { get; private set; }
        protected UIDomainApi domainApi;

        public abstract UILayerType layerType { get; }
        public abstract string uiPkgUrl { get; }
        public abstract string uiName { get; }
        public string uiUrl => $"{uiPkgUrl}/{uiName}";

        public UIStateType state { get; private set; }

        protected UIViewInput _viewInput;

        public UIBase()
        {
        }

        public void Inject(GameObject go, UIDomainApi domainApi)
        {
            this.go = go;
            this.domainApi = domainApi;
        }

        /// <summary>
        /// 初始化, 定义UI的预制体信息, 以及界面传参
        /// </summary>
        public void Init(UIViewInput viewInput)
        {
            this._viewInput = viewInput;
            _OnInit();
            this.state = UIStateType.Inited;
        }
        protected virtual void _OnInit() { }

        /// <summary>
        /// 显示
        /// <para>inputArgs: 输入参数</para>
        /// </summary>
        public void Show(UIViewInput viewInput = null)
        {
            if (viewInput != null) this._viewInput = viewInput;
            _OnShow();
            // 根据UI类型, 区分不同的显示方式
            switch (layerType)
            {
                case UILayerType.Window:
                    break;
                case UILayerType.PopUp:
                    break;
                default:
                    break;
            }
            this.state = UIStateType.Showed;
        }
        protected virtual void _OnShow() { }

        public void Hide()
        {
            if (this.state == UIStateType.Hided) return;
            _OnHide();
            this.state = UIStateType.Hided;
        }
        protected virtual void _OnHide() { }

        public void Destroy()
        {
            if (this.state == UIStateType.Destroyed) return;
            this._RemoveAllTimer();
            _OnDestroy();
            this.state = UIStateType.Destroyed;
            GameObject.Destroy(this.go);
            this.go = null;
            this._viewInput.closeAction?.Invoke();
        }
        protected virtual void _OnDestroy() { }

        #region [Timer]
        private List<int> _timerIdList;
        public void SetInterval(float interval, Action callback)
        {
            domainApi.directApi.SetInterval(interval, callback);
        }
        public void RemoveTimer(int timerId)
        {
            domainApi.directApi.RemoveTimer(timerId);
        }
        private void _RemoveAllTimer()
        {
            this._timerIdList?.Foreach((timerId) =>
            {
                RemoveTimer(timerId);
            });
        }
        #endregion

        protected void _AddClick(GameObject go, Action callback)
        {
            var clickCom = go.GetComponent<UIClickCom>() ?? go.AddComponent<UIClickCom>();
            clickCom.onClick = callback;
        }

        protected void _Close()
        {
            domainApi.directApi.CloseUI(this.uiName);
        }
    }
}