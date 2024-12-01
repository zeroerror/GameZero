using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public static class GameProjectileRCCollection
    {
        /** 弹道 - 创建 */
        public static readonly string RC_GAME_PROJECTILE_CREATE = "RC_GAME_PROJECTILE_CREATE";
        /* 弹道状态 - 进入 待机 */
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_IDLE = "RC_GAME_PROJECTILE_STATE_ENTER_IDLE";
        /* 弹道状态 - 进入 固定方向飞行 */
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_FIXED_DIRECTION = "RC_GAME_PROJECTILE_STATE_ENTER_FIXED_DIRECTION";
        /* 弹道状态 - 进入 锁定实体飞行 */
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_ENTITY = "RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_ENTITY";
        /* 弹道状态 - 进入 锁定位置飞行 */
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_POSITION = "RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_POSITION";
        /* 弹道状态 - 进入 附着 */
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_ATTACH = "RC_GAME_PROJECTILE_STATE_ENTER_ATTACH";
        /* 弹道状态 - 进入 爆炸 */
        public static readonly string RC_GAME_PROJECTILE_STATE_ENTER_EXPLODE = "RC_GAME_PROJECTILE_STATE_ENTER_EXPLODE";
    }

    public struct GameProjectileRCArgs_Create
    {
        public GameIdArgs idArgs;
        public GameIdArgs creatorIdArgs;
        public GameTransformArgs transArgs;
    }

    public struct GameProjectileRCArgs_StateEnterIdle
    {
        public GameIdArgs idArgs;
    }

    public struct GameProjectileRCArgs_StateEnterFixedDirection
    {
        public GameIdArgs idArgs;
        public GameVec2 direction;
    }

    public struct GameProjectileRCArgs_StateEnterLockOnEntity
    {
        public GameIdArgs idArgs;
        public GameIdArgs targetIdArgs;
    }

    public struct GameProjectileRCArgs_StateEnterLockOnPosition
    {
        public GameIdArgs idArgs;
        public GameVec2 targetPosition;
    }

    public struct GameProjectileRCArgs_StateEnterAttach
    {
        public GameIdArgs idArgs;
        // 附着的地点坐标
        public GameVec2 pos;
        // 或者附着的目标及偏移
        public GameIdArgs targetIdArgs;
        public GameVec2 offset;
    }

    public struct GameProjectileRCArgs_StateEnterExplode
    {
        public GameIdArgs idArgs;
    }
}
