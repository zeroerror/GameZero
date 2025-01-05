using System;
using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public abstract class GameUIBase
    {
        public GameObject go { get; private set; }
        private GameUIDomainApi domainApi;

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
        protected abstract void _OnInit();

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
        protected abstract void _OnShow();

        public void Hide()
        {
            if (this.state == GameUIStateType.Hided) return;
            _OnHide();
            this._RemoveAllTimer();
            this.state = GameUIStateType.Hided;
        }
        protected abstract void _OnHide();

        public void Destroy()
        {
            if (this.state == GameUIStateType.Destroyed) return;
            _OnDestroy();
            this.state = GameUIStateType.Destroyed;
        }
        protected abstract void _OnDestroy();

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

    }
}