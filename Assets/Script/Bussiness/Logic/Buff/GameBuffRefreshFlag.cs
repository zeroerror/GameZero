namespace GamePlay.Bussiness.Logic
{
    [System.Flags]
    public enum GameBuffRefreshFlag
    {
        None,
        /// <summary> 刷新时间 </summary>
        RefreshTime = 1 << 0,
        /// <summary> 叠加时间 </summary>
        StackTime = 1 << 1,
        /// <summary> 叠加层数 </summary>
        StackLayer = 1 << 2,
    }
}