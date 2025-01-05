namespace GamePlay.Bussiness.UI
{
    public enum GameUIStateType
    {
        None = 0,
        /// <summary> 已初始化 </summary>
        Inited = 1,
        /// <summary> 已显示 </summary>
        Showed = 2,
        /// <summary> 已隐藏 </summary>
        Hided = 3,
        /// <summary> 已销毁 </summary>
        Destroyed = 4,
    }
}