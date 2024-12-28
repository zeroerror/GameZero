namespace GamePlay.Bussiness.Logic
{
    public struct GameActionRecord_SummonRoles
    {
        /// <summary> 行为Id </summary>
        public int actionId;
        /// <summary> 行为者角色Id参数, 不一定存在 </summary>
        public GameIdArgs actorRoleIdArgs;
        /// <summary> 行为者Id参数, 比如技能, 投射物, BUFF等 </summary>
        public GameIdArgs actorIdArgs;

        /// <summary> 角色类型Id </summary>
        public int roleId;
        /// <summary> 阵营类型 </summary>
        public GameCampType campType;
        /// <summary> 层数 </summary>
        public int count;

        public GameActionRecord_SummonRoles(
            int actionId,
            in GameIdArgs actorRoleIdArgs,
            in GameIdArgs actorIdArgs,
            int roleId,
            GameCampType campType,
            int count
        )
        {
            this.actionId = actionId;
            this.actorRoleIdArgs = actorRoleIdArgs;
            this.actorIdArgs = actorIdArgs;
            this.roleId = roleId;
            this.campType = campType;
            this.count = count;
        }
    }
}