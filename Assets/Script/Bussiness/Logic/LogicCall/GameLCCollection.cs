namespace GamePlay.Bussiness.Logic
{
    public static class GameLCCollection
    {
        /// <summary> 选择行为选项 </summary>
        public static readonly string LC_GAME_ACTION_OPTION_SELECTED = "LC_GAME_ACTION_OPTION_SELECTED";
        /// <summary> 准备阶段 - 确认战斗 </summary>
        public static readonly string LC_GAME_PREPARING_CONFIRM_FIGHT = "LC_GAME_PREPARING_CONFIRM_FIGHT";
        /// <summary> 结算阶段 - 确认退出 </summary>
        public static readonly string LC_GAME_SETTLING_CONFIRM_EXIT = "LC_GAME_SETTLING_CONFIRM_EXIT";
    }

    public struct GameLCArgs_ActionOptionSelected
    {
        public int optionId;

        public GameLCArgs_ActionOptionSelected(int optionId)
        {
            this.optionId = optionId;
        }
    }

    public struct GameLCArgs_PreparingConfirmFight
    {
        // 确定本次战斗的内容如
        // 难度
        // 敌方buff
        // ......
    }

    public struct GameLCArgs_SettlingConfirmExit
    {
    }
}
