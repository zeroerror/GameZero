namespace GamePlay.Bussiness.Logic
{
    public static class GameSkillRCCollection
    {
        /** 技能 - 创建 */
        public static readonly string RC_GAMES_SKILL_CREATE = "RC_GAMES_SKILL_CREATE";
        /** 技能 - 切换 */
        public static readonly string RC_GAMES_SKILL_TRANSFORM = "RC_GAMES_SKILL_SWITCH";
    }

    public struct GameSkillRCArgs_Create
    {
        public GameIdArgs skillIdArgs;
        public GameIdArgs roleIdArgs;
    }

    public struct GameSkillRCArgs_CharacterTransform
    {
        public GameIdArgs[] skillIdArgsList;
        public GameIdArgs roleIdArgs;
    }

}
