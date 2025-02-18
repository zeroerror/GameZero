namespace GamePlay.Bussiness.Logic
{
    public static class GameBuffRCCollection
    {
        /// <summary>  Buff - 挂载 </summary>
        public static readonly string RC_GAME_BUFF_ATTACH = "RC_GAME_BUFF_ATTACH";
        /// <summary>  Buff - 移除 </summary>
        public static readonly string RC_GAME_BUFF_DETACH = "RC_GAME_BUFF_DETACH";
    }

    public struct GameBuffRCArgs_Attach
    {
        public GameIdArgs buffIdArgs;
        public GameIdArgs targetIdArgs;
        public int layer;
    }

    public struct GameBuffRCArgs_Detach
    {
        public int buffId;
        public GameIdArgs targetIdArgs;
        public int detachLayer;
    }
}