namespace GamePlay.Bussiness.Logic
{
    public static class GameAttributeRCCollection
    {
        /** 当前属性组件同步 */
        public static readonly string RC_GAME_ATTRIBUTE_SYNC = "RC_GAME_ATTRIBUTE_SYNC";
        /** 基础属性组件同步 */
        public static readonly string RC_GAME_ATTRIBUTE_BASE_SYNC = "RC_GAME_ATTRIBUTE_BASE_SYNC";
    }

    public struct GameAttributeRCArgs_Sync
    {
        public GameIdArgs idArgs;
        public GameAttributeArgs attrAgrs;
    }

    public struct GameAttributeRCArgs_BaseSync
    {
        public GameIdArgs idArgs;
        public GameAttributeArgs attrAgrs;
    }
}
