namespace GamePlay.Bussiness.Logic
{
    public interface GameDirectorDomainApi
    {
        public GameDirectorFSMDomainApi fsmApi { get; }

        /// <summary>
        /// 设置逻辑层的时间缩放
        /// </summary>
        public void SetTimeScale(float timeScale);

        /// <summary>
        /// 驱动渲染调用(RC)事件
        /// </summary>
        public void TickRCEvents();

        /// <summary>
        /// 提交事件
        /// </summary>
        public void SubmitEvent(string eventName, object args);

        /// <summary>
        /// 绑定RC事件
        /// </summary>
        public void BindRC(string rcName, System.Action<object> callback);

        /// <summary>
        /// 解绑RC事件
        /// </summary>
        public void UnbindRC(string rcName, System.Action<object> callback);
    }
}