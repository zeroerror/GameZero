namespace GamePlay.Bussiness.Logic
{
    public static class GameFieldRCCollection
    {
        /** 场地 - 创建 */
        public static readonly string RC_GAME_FIELD_CREATE = "RC_GAME_FIELD_CREATE";
    }

    public struct GameFieldRCArgs_Create
    {
        public int typeId;
    }
}