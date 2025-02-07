using GameVec2 = UnityEngine.Vector2;
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
        /// <summary> 单位 站位改变 </summary>
        public static readonly string LC_GAME_UNIT_POSITION_CHANGED = "LC_GAME_UNIT_POSITION_CHANGED";
    }

    /// <summary> 参数 - 选择行为选项 </summary>
    public struct GameLCArgs_ActionOptionSelected
    {
        public int optionId;

        public GameLCArgs_ActionOptionSelected(int optionId)
        {
            this.optionId = optionId;
        }
    }

    /// <summary> 参数 - 准备阶段 - 确认战斗 </summary>
    public struct GameLCArgs_PreparingConfirmFight
    {
        // 确定本次战斗的内容如
        // 难度
        // 敌方buff
        // ......
    }

    /// <summary> 参数 - 结算阶段 - 确认退出 </summary>
    public struct GameLCArgs_SettlingConfirmExit
    {
    }

    /// <summary> 参数 - 单位 站位改变 </summary>
    public struct GameLCArgs_UnitPositionChanged
    {
        public GameEntityType entityType;
        public int entityId;
        public GameVec2 newPosition;
    }
}
