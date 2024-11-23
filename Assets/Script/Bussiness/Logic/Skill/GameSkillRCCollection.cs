namespace GamePlay.Bussiness.Logic
{
    public static class GameSkillRCCollection
    {
        /** 技能 - 创建 */
        public static readonly string RC_GAMES_SKILL_CREATE = "RC_GAMES_SKILL_CREATE";
    }

    public struct GameSkillRCArgs_Create
    {
        public GameIdArgs skillIdArgs;
        public GameIdArgs roleIdArgs;
    }

}
