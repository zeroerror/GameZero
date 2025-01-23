namespace GamePlay.Bussiness.Logic
{
    public static class GameLCCollection
    {
        /// <summary> 选择行为选项 </summary>
        public static readonly string LC_GAME_ACTION_OPTION_SELECTED = "LC_GAME_ACTION_OPTION_SELECTED";
    }

    public struct GameLCArgs_ActionOptionSelected
    {
        public int optionId;

        public GameLCArgs_ActionOptionSelected(int optionId)
        {
            this.optionId = optionId;
        }
    }
}
