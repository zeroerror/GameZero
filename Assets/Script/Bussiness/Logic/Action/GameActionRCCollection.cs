namespace GamePlay.Bussiness.Logic
{
    public static class GameActionRCCollection
    {
        /** 行为 - 执行 */
        public static readonly string RC_GAME_ACTION_DO = "RC_GAME_ACTION_DO";
    }

    public struct GameActionRCArgs_Do
    {
        public int actionId;
        public GameIdArgs actorIdArgs;
        public GameIdArgs targetIdArgs;
    }

}
