namespace GamePlay.Bussiness.Logic
{
    public static class GameFieldRCCollection
    {
        /// <summary> 场地 - 创建 </summary>
        public static readonly string RC_GAME_FIELD_CREATE = "RC_GAME_FIELD_CREATE";
        /// <summary> 场地 - 清理 </summary>
        public static readonly string RC_GAME_FIELD_CLEAR = "RC_GAME_FIELD_CLEAR";
        /// <summary> 场地 - 销毁 </summary>
        public static readonly string RC_GAME_FIELD_DESTROY = "RC_GAME_FIELD_DESTROY";
    }

    public struct GameFieldRCArgs_Create
    {
        public int typeId;
    }

    public struct GameFieldRCArgs_Clear
    {
        public int typeId;
    }

    public struct GameFieldRCArgs_Destroy
    {
        public int typeId;
    }
}