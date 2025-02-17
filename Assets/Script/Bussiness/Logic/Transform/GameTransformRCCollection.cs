namespace GamePlay.Bussiness.Logic
{
    public static class GameTransformRCCollection
    {
        /** 同步变换 */
        public static readonly string RC_GAME_TRANSFORMN_SYNC = "RC_GAME_TRANSFORMN_SYNC";
        /** 立即同步变换 */
        public static readonly string RC_GAME_TRANSFORMN_SYNC_IMMEDIATE = "RC_GAME_TRANSFORMN_SYNC_IMMEDIATE";
    }

    public struct GameTransformRCArgs_Sync
    {
        public GameIdArgs idArgs;
        public GameTransformArgs transArgs;
    }

    public struct GameTransformRCArgs_SyncImmediate
    {
        public GameIdArgs idArgs;
        public GameTransformArgs transArgs;
    }

}
