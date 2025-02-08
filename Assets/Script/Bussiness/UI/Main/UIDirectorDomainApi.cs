using System;
using GamePlay.Bussiness.Core;
using UnityEngine;
namespace GamePlay.Bussiness.UI
{
    public interface UIDirectorDomainApi
    {
        /// <summary>
        /// 打开UI
        /// <para>viewInput: 界面传参</para>
        /// </summary>
        public void OpenUI<T>(UIViewInput viewInput = null) where T : UIBase;

        /// <summary>
        /// 关闭UI
        /// </summary>
        public void CloseUI(string uiName);

        /// <summary>
        /// 关闭UI
        /// </summary>
        public void CloseUI<T>() where T : UIBase;

        /// <summary>
        /// 设置定时器
        /// <para>interval: 间隔时间(s)</para>
        /// <para>callback: 回调函数</para>
        /// </summary>
        public int SetInterval(float interval, Action callback);

        /// <summary>
        /// 设置延迟器
        /// <para>delay: 延迟时间(s)</para>
        /// <para>callback: 回调函数</para>
        /// </summary>
        public int SetTimeout(float delay, Action callback);

        /// <summary>
        /// 移除定时器
        /// </summary>
        public void RemoveTimer(int timerId);

        /// <summary>
        /// 绑定按键事件
        /// <para>keyCode: 按键</para>
        /// <para>callback: 回调函数</para>
        /// <para>stateType: 按键状态</para>
        /// </summary>
        public void BindKeyAction(KeyCode keyCode, Action callback, GameInputStateType stateType = GameInputStateType.KeyDown);

        /// <summary>
        /// 解绑按键事件
        /// <para>keyCode: 按键</para>
        /// <para>callback: 回调函数</para>
        /// <para>stateType: 按键状态</para>
        /// </summary>
        public void UnbindKeyAction(KeyCode keyCode, Action callback, GameInputStateType stateType = GameInputStateType.KeyDown);

        /// <summary>
        /// 绑定事件
        /// <para>eventName: 事件名</para>
        /// <para>callback: 回调函数</para>
        /// </summary>
        public void BindEvent(string eventName, Action<object> callback);

        /// <summary>
        /// 解绑事件
        /// <para>eventName: 事件名</para>
        /// <para>callback: 回调函数</para>
        /// </summary>
        public void UnbindEvent(string eventName, Action<object> callback);

        /// <summary>
        /// 获取鼠标位置
        /// </summary>
        public Vector3 GetPointerPosition();
    }
}