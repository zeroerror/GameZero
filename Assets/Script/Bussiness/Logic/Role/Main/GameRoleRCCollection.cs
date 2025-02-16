namespace GamePlay.Bussiness.Logic
{
    public static class GameRoleRCCollection
    {
        /// <summary> 角色 - 创建 </summary>
        public static readonly string RC_GAME_ROLE_CREATE = "RC_GAME_ROLE_CREATE";
        /// <summary> 角色 - 变身 </summary>    
        public static readonly string RC_GAME_ROLE_TRANSFORM = "RC_GAME_ROLE_TRANSFORM";
        /// <summary> 角色 - 状态退出 </summary>
        public static readonly string RC_GAME_ROLE_STATE_EXIT = "RC_GAME_ROLE_STATE_EXIT";
        /// <summary> 角色 - 进入 待机 </summary>
        public static readonly string RC_GAME_ROLE_STATE_ENTER_IDLE = "RC_GAME_ROLE_STATE_ENTER_IDLE";
        /// <summary> 角色 - 进入 移动 </summary>
        public static readonly string RC_GAME_ROLE_STATE_ENTER_MOVE = "RC_GAME_ROLE_STATE_ENTER_MOVE";
        /// <summary> 角色 - 进入 施法 </summary>
        public static readonly string RC_GAME_ROLE_STATE_ENTER_CAST = "RC_GAME_ROLE_STATE_ENTER_CAST";
        /// <summary> 角色 - 进入 死亡 </summary>
        public static readonly string RC_GAME_ROLE_STATE_ENTER_DEAD = "RC_GAME_ROLE_STATE_ENTER_DEAD";
        /// <summary> 角色 - 进入 隐身 </summary>
        public static readonly string RC_GAME_ROLE_STATE_ENTER_STEALTH = "RC_GAME_ROLE_STATE_ENTER_STEALTH";
        /// <summary> 角色 - 隐身 - 移动 </summary>
        public static readonly string RC_GAME_ROLE_STATE_STEALTH_MOVE = "RC_GAME_ROLE_STATE_STEALTH_MOVE";
        /// <summary> 角色 - 进入 摧毁 </summary>
        public static readonly string RC_GAME_ROLE_STATE_ENTER_DESTROYED = "RC_GAME_ROLE_STATE_ENTER_DESTROYED";
    }

    /// <summary> 参数 - 角色 - 创建 </summary>
    public struct GameRoleRCArgs_Create
    {
        public GameIdArgs idArgs;
        public GameTransformArgs transArgs;
        public bool isEnemy;
    }

    /// <summary> 参数 - 角色 - 变身 </summary>
    public struct GameRoleRCArgs_CharacterTransform
    {
        public GameIdArgs idArgs;
    }

    /// <summary> 参数 - 角色 - 状态退出 </summary>
    public struct GameRoleRCArgs_StateExit
    {
        public GameRoleStateType exitStateType;
        public GameIdArgs idArgs;
    }

    /// <summary> 参数 - 角色 - 进入 待机 </summary>
    public struct GameRoleRCArgs_StateEnterIdle
    {
        public GameRoleStateType fromStateType;
        public GameIdArgs idArgs;
    }

    /// <summary> 参数 - 角色 - 进入 移动 </summary>
    public struct GameRoleRCArgs_StateEnterMove
    {
        public GameRoleStateType fromStateType;
        public GameIdArgs idArgs;
    }

    /// <summary> 参数 - 角色 - 进入 施法 </summary>
    public struct GameRoleRCArgs_StateEnterCast
    {
        public GameRoleStateType fromStateType;
        public GameIdArgs idArgs;
        public int skillId;
    }

    /// <summary> 参数 - 角色 - 进入 死亡 </summary>
    public struct GameRoleRCArgs_StateEnterDead
    {
        public GameRoleStateType fromStateType;
        public GameIdArgs idArgs;
    }

    /// <summary> 参数 - 角色 - 进入 隐身 </summary>
    public struct GameRoleRCArgs_StateEnterStealth
    {
        public GameRoleStateType fromStateType;
        public GameIdArgs idArgs;
    }

    /// <summary> 参数 - 角色 - 隐身 - 移动 </summary>
    public struct GameRoleRCArgs_StateStealthMove
    {
        public GameIdArgs idArgs;
        public bool isMoving;
    }

    /// <summary> 参数 - 角色 - 进入 已销毁 </summary>
    public struct GameRoleRCArgs_StateEnterDestroyed
    {
        public GameRoleStateType fromStateType;
        public GameIdArgs idArgs;
    }
}