namespace GamePlay.Bussiness.Logic
{
    public enum GameBuffRefreshFlag
    {
        None,
        /// <summary> 刷新时间 </summary>
        RefreshTime = 1 << 0,
        /// <summary> 叠加时间 </summary>
        StackTime = 1 << 1,
        /// <summary> 叠加层数 </summary>
        StackLayer = 1 << 2,
        /// <summary> 叠加数值 </summary>
        StackValue = 1 << 3,
    }
}