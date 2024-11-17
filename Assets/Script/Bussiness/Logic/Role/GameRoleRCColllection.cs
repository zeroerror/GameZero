namespace GamePlay.Bussiness.Logic
{
    public static class GameRoleRCColllection
    {
        /* 角色状态 - 进入 空闲 */
        public static readonly string RC_GAME_ROLE_STATE_ENTER_IDLE = "RC_GAME_ROLE_STATE_ENTER_IDLE";
        public struct GameRoleRCArgs_StateEnterIdle
        {
            public GameRoleStateType fromState;
            public GameIdComArgs idComArg;
        }

        /* 角色状态 - 进入 移动 */
        public static readonly string RC_GAME_ROLE_STATE_ENTER_MOVE = "RC_GAME_ROLE_STATE_ENTER_MOVE";
        public struct GameRoleRCArgs_StateEnterMove
        {
            public GameRoleStateType fromState;
            public GameIdComArgs idComArg;
        }

        /* 角色状态 - 进入 施法 */
        public static readonly string RC_GAME_ROLE_STATE_ENTER_CAST = "RC_GAME_ROLE_STATE_ENTER_CAST";
        public struct GameRoleRCArgs_StateEnterCast
        {
            public GameRoleStateType fromState;
            public GameIdComArgs idComArg;
        }

        /* 角色状态 - 进入 死亡 */
        public static readonly string RC_GAME_ROLE_STATE_ENTER_DEAD = "RC_GAME_ROLE_STATE_ENTER_DEAD";
        public struct GameRoleRCArgs_StateEnterDead
        {
            public GameRoleStateType fromState;
            public GameIdComArgs idComArg;
        }
    }
}
