using System;
using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public abstract class GameUIBase
    {
        public GameObject go { get; private set; }
        protected GameUIDomainApi domainApi;

        public abstract GameUILayerType layerType { get; }
        public abstract string uiPkgUrl { get; }
        public abstract string uiName { get; }
        public string uiUrl => $"{uiPkgUrl}/{uiName}";

        public GameUIStateType state { get; private set; }

        protected object _inputArgs;

        public GameUIBase()
        {
        }

        public void Inject(GameObject go, GameUIDomainApi domainApi)
        {
            this.go = go;
            this.domainApi = domainApi;
        }

        /// <summary>
        /// 初始化, 定义UI的预制体信息, 以及界面传参
        /// </summary>
        public void Init(object inputArgs)
        {
            this._inputArgs = inputArgs;
            _OnInit();
            this.state = GameUIStateType.Inited;
        }
        protected virtual void _OnInit() { }

        /// <summary>
        /// 显示
        /// <para>inputArgs: 输入参数</para>
        /// </summary>
        public void Show(object inputArgs = null)
        {
            if (inputArgs != null) this._inputArgs = inputArgs;
            _OnShow();
            // 根据UI类型, 区分不同的显示方式
            switch (layerType)
            {
                case GameUILayerType.Window:
                    break;
                case GameUILayerType.PopUp:
                    break;
                default:
                    break;
            }
            this.state = GameUIStateType.Showed;
        }
        protected virtual void _OnShow() { }

        public void Hide()
        {
            if (this.state == GameUIStateType.Hided) return;
            _OnHide();
            this.state = GameUIStateType.Hided;
        }
        protected virtual void _OnHide() { }

        public void Destroy()
        {
            if (this.state == GameUIStateType.Destroyed) return;
            this._RemoveAllTimer();
            _OnDestroy();
            this.state = GameUIStateType.Destroyed;
            GameObject.Destroy(this.go);
            this.go = null;
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
            var clickCom = go.GetComponent<GameUIClickCom>() ?? go.AddComponent<GameUIClickCom>();
            clickCom.onClick = callback;
        }

        protected void _Close()
        {
            domainApi.directApi.CloseUI(this.uiName);
        }
    }
}