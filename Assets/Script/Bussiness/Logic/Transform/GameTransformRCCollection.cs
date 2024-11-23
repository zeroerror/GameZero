namespace GamePlay.Bussiness.Logic
{
    public static class GameTransformRCCollection
    {
        /** 组件同步 */
        public static readonly string RC_GAME_TRANSFORMN_SYNC = "RC_GAME_TRANSFORMN_SYNC";
    }

    public struct GameTransformRCArgs_Sync
    {
        public GameIdArgs idArgs;
        public GameTransformArgs transComArgs;
    }
}
