namespace GamePlay.Bussiness.Logic
{
    public enum GameDirectorStateType
    {
        /// <summary> 无 </summary>
        None = 0,
        /// <summary> 加载中 </summary>
        Loading = 1,
        /// <summary> 战斗准备 </summary>
        FightPreparing = 2,
        /// <summary> 战斗中 </summary>
        Fighting = 3,
        /// <summary> 结算中 </summary>
        Settling = 4,
    }
}