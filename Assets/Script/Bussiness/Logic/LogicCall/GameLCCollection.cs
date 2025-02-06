namespace GamePlay.Bussiness.Logic
{
    public static class GameLCCollection
    {
        /// <summary> 选择行为选项 </summary>
        public static readonly string LC_GAME_ACTION_OPTION_SELECTED = "LC_GAME_ACTION_OPTION_SELECTED";
        /// <summary> 准备阶段 - 确认开始 </summary>
        public static readonly string LC_GAME_PREPARING_CONFIRM_START = "LC_GAME_PREPARING_CONFIRM_START";
    }

    public struct GameLCArgs_ActionOptionSelected
    {
        public int optionId;

        public GameLCArgs_ActionOptionSelected(int optionId)
        {
            this.optionId = optionId;
        }
    }

    public struct GameLCArgs_PreparingConfirmStart
    {
        // 确定本次战斗的内容如
        // 难度
        // 敌方buff
        // ......
    }
}
