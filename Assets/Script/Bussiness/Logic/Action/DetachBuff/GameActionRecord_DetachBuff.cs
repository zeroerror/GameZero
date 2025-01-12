namespace GamePlay.Bussiness.Logic
{
    public struct GameActionRecord_DetachBuff
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
        /// <summary> buff类型Id </summary>
        public int buffId;
        /// <summary> 层数 </summary>
        public int layer;

        public GameActionRecord_DetachBuff(
            int actionId,
            in GameIdArgs actorRoleIdArgs,
            in GameIdArgs actorIdArgs,
            in GameIdArgs targetRoleIdArgs,
            in GameActionTargeterArgsRecord actionTargeter,
            int buffId,
            int layer
        )
        {
            this.actionId = actionId;
            this.actorRoleIdArgs = actorRoleIdArgs;
            this.actorIdArgs = actorIdArgs;
            this.targetIdArgs = targetRoleIdArgs;
            this.actionTargeter = actionTargeter;
            this.buffId = buffId;
            this.layer = layer;
        }
    }
}