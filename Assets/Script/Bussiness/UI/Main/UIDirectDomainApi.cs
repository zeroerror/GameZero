namespace GamePlay.Bussiness.UI
{
    public interface UIDirectDomainApi
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
        /// 设置定时器
        /// <para>interval: 间隔时间(s)</para>
        /// <para>callback: 回调函数</para>
        /// </summary>
        public int SetInterval(float interval, System.Action callback);

        /// <summary>
        /// 设置延迟器
        /// <para>delay: 延迟时间(s)</para>
        /// <para>callback: 回调函数</para>
        /// </summary>
        public int SetTimeout(float delay, System.Action callback);

        /// <summary>
        /// 移除定时器
        /// </summary>
        public void RemoveTimer(int timerId);
    }
}