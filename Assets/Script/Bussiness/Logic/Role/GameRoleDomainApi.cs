namespace GamePlay.Bussiness.Logic
{
    public interface GameRoleDomainApi
    {
        public GameRoleFSMDomainApi fsmApi { get; }
        public GameRoleAIDomainApi aiApi { get; }

        /// <summary>
        /// 创建角色
        /// <para>typeId: 类型Id</para>
        /// <para>campId: 阵营Id</para>
        /// <para>transArgs: 变换参数</para>
        /// <para>isUser: 是否为用户 </para>
        /// </summary>
        public GameRoleEntity CreateRole(int typeId, int campId, in GameTransformArgs transArgs, bool isUser);

        /// <summary>
        /// 创建玩家角色
        /// <para>typeId: 类型Id</para>
        /// <para>transArgs: 变换参数</para>
        /// <para>isUser: 是否为用户 </para>
        /// </summary>
        public GameRoleEntity CreatePlayerRole(int typeId, in GameTransformArgs transArgs, bool isUser);

        /// <summary>
        /// 创建怪物角色
        /// <para>typeId: 类型Id</para>
        /// <para>transArgs: 变换参数</para>
        /// </summary>
        public GameRoleEntity CreateMonsterRole(int typeId, in GameTransformArgs transArgs);

        /// <summary>
        /// 获取最近的敌人
        /// <para>entity: 实体</para>
        /// </summary>
        public GameRoleEntity GetNearestEnemy(GameEntityBase entity);

        /// <summary>
        /// 召唤角色
        /// <para>model: 召唤角色行为模型</para>
        /// <para>summoner: 召唤者</para>
        /// <para>transArgs: 变换参数</para>
        /// </summary>
        public GameRoleEntity[] SummonRoles(GameActionModel_SummonRoles action, GameEntityBase summoner, in GameTransformArgs transArgs);

        /// <summary>
        /// 召唤角色
        /// <para>summoner: 召唤者</para>
        /// <para>typeId: 类型Id</para>
        /// <para>campType: 阵营类型</para>
        /// <para>count: 个数</para>
        /// <para>transArgs: 变换参数</para>
        /// </summary>
        public GameRoleEntity[] SummonRoles(
            GameEntityBase summoner,
            int typeId,
            GameCampType campType,
            int count,
            in GameTransformArgs transArgs
        );
    }
}
