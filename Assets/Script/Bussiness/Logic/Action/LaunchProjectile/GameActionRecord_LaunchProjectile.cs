using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameActionRecord_LaunchProjectile
    {
        /// <summary> 行为Id </summary>
        public int actionId;
        /// <summary> 行为者角色Id参数, 不一定存在 </summary>
        public GameIdArgs actorRoleIdArgs;
        /// <summary> 行为者Id参数, 比如技能, 投射物, BUFF等 </summary>
        public GameIdArgs actorIdArgs;
        /// <summary> 目标角色Id参数 </summary>
        public GameIdArgs targetIdArgs;
        /// <summary> 行为目标选取器参数记录 </summary>
        public GameActionTargeterArgsRecord actionTargeter;

        public GameActionRecord_LaunchProjectile(
            int actionId,
            in GameIdArgs actorRoleIdArgs,
            in GameIdArgs actorIdArgs,
            in GameIdArgs targetRoleIdArgs,
            in GameActionTargeterArgsRecord actionTargeter
        )
        {
            this.actionId = actionId;
            this.actorRoleIdArgs = actorRoleIdArgs;
            this.actorIdArgs = actorIdArgs;
            this.targetIdArgs = targetRoleIdArgs;
            this.actionTargeter = actionTargeter;
        }
    }
}