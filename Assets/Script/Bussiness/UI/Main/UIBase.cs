using System;
using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public abstract class UIBase
    {
        public GameObject go { get; private set; }
        protected UIDomainApi _uiApi { get; private set; }

        public abstract UILayerType layerType { get; }
        public abstract string uiPkgUrl { get; }
        public abstract string uiName { get; }
        public string uiUrl => $"{uiPkgUrl}/{uiName}";

        public UIStateType state { get; private set; }

        protected UIViewInput _uiInput;

        public UIBase() { }

        #region [生命周期]

        public void Inject(GameObject go, UIDomainApi domainApi)
        {
            this.go = go;
            this._uiApi = domainApi;
        }

        /// <summary>
        /// 初始化, 定义UI的预制体信息, 以及界面传参
        /// </summary>
        public void Init(UIViewInput viewInput)
        {
            this._uiInput = viewInput;
            _OnInit();
            this.state = UIStateType.Inited;
            this._BindEvents();
        }
        protected virtual void _OnInit() { }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            if (this.state == UIStateType.Destroyed) return;
            this._RemoveAllTimer();
            _OnDestroy();
            this.state = UIStateType.Destroyed;
            GameObject.Destroy(this.go);
            this.go = null;
            this._uiInput.closeAction?.Invoke();
            this._UnbindEvents();
        }
        protected virtual void _OnDestroy() { }

        /// <summary>
        /// 绑定事件
        /// </summary>
        protected virtual void _BindEvents() { }

        /// <summary>
        /// 解绑事件
        /// </summary>
        protected virtual void _UnbindEvents() { }

        /// <summary>
        /// 显示UI
        /// <para>inputArgs: 输入参数</para>
        /// </summary>
        public void Show(UIViewInput viewInput = null)
        {
            if (viewInput != null) this._uiInput = viewInput;
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

        /// <summary>
        /// 隐藏UI
        /// </summary>
        public void Hide()
        {
            if (this.state == UIStateType.Hided) return;
            _OnHide();
            this.state = UIStateType.Hided;
        }
        protected virtual void _OnHide() { }

        #endregion

        #region [定时器]
        private List<int> _timerIdList;
        public int SetInterval(float interval, Action callback)
        {
            var id = _uiApi.directorApi.SetInterval(interval, callback);
            _timerIdList = _timerIdList ?? new List<int>();
            _timerIdList.Add(id);
            return id;
        }
        public void RemoveTimer(int timerId)
        {
            _uiApi.directorApi.RemoveTimer(timerId);
        }
        private void _RemoveAllTimer()
        {
            this._timerIdList?.Foreach((timerId) =>
            {
                RemoveTimer(timerId);
            });
        }
        #endregion

        #region [通用]
        public void SetClick(GameObject go, Action callback)
        {
            var clickCom = go.GetComponent<UIClickCom>() ?? go.AddComponent<UIClickCom>();
            clickCom.onClick = callback;
        }

        protected void _Close()
        {
            _uiApi.directorApi.CloseUI(this.uiName);
        }
        #endregion
    }
}