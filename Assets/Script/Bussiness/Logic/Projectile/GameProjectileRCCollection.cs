using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public static class GameProjectileRCCollection
    {
        /// <summary> 弹道 - 创建  </summary>
        public static readonly string RC_GAME_PROJECTILE_CREATE = "RC_GAME_PROJECTILE_CREATE";
        /// <summary> 弹道状态 - 进入 待机  </summary>
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_IDLE = "RC_GAME_PROJECTILE_STATE_ENTER_IDLE";
        /// <summary> 弹道状态 - 进入 固定方向飞行  </summary>
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_FIXED_DIRECTION = "RC_GAME_PROJECTILE_STATE_ENTER_FIXED_DIRECTION";
        /// <summary> 弹道状态 - 进入 锁定实体飞行  </summary>
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_ENTITY = "RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_ENTITY";
        /// <summary> 弹道状态 - 进入 锁定地点飞行  </summary>
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_POSITION = "RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_POSITION";
        /// <summary> 弹道状态 - 进入 附着  </summary>
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_ATTACH = "RC_GAME_PROJECTILE_STATE_ENTER_ATTACH";
        /// <summary> 弹道状态 - 进入 爆炸  </summary>
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_EXPLODE = "RC_GAME_PROJECTILE_STATE_ENTER_EXPLODE";
        /// <summary> 弹道状态 - 进入 摧毁  </summary>
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_DESTROYED = "RC_GAME_PROJECTILE_STATE_ENTER_DESTROYED";

    }

    public struct GameProjectileRCArgs_Create
    {
        public GameIdArgs idArgs;
        public GameIdArgs creatorIdArgs;
        public GameTransformArgs transArgs;
    }

    public struct GameProjectileRCArgs_StateEnterIdle
    {
        public GameProjectileStateType fromStateType;
        public GameIdArgs idArgs;
    }

    public struct GameProjectileRCArgs_StateEnterFixedDirection
    {
        public GameProjectileStateType fromStateType;
        public GameIdArgs idArgs;
        public GameVec2 direction;
    }

    public struct GameProjectileRCArgs_StateEnterLockOnEntity
    {
        public GameProjectileStateType fromStateType;
        public GameIdArgs idArgs;
        public GameIdArgs targetIdArgs;
    }

    public struct GameProjectileRCArgs_StateEnterLockOnPosition
    {
        public GameProjectileStateType fromStateType;
        public GameIdArgs idArgs;
        public GameVec2 targetPosition;
    }

    public struct GameProjectileRCArgs_StateEnterAttach
    {
        public GameProjectileStateType fromStateType;
        public GameIdArgs idArgs;
        // 附着的地点坐标
        public GameVec2 pos;
        // 或者附着的目标
        public GameIdArgs targetIdArgs;
    }

    public struct GameProjectileRCArgs_StateEnterExplode
    {
        public GameProjectileStateType fromStateType;
        public GameIdArgs idArgs;
    }

    public struct GameProjectileRCArgs_StateEnterDestroyed
    {
        public GameProjectileStateType fromStateType;
        public GameIdArgs idArgs;
    }
}
