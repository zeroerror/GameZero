namespace GamePlay.Bussiness.Render
{
    public interface GameRoleDomainApiR
    {
        public GameRoleFSMDomainApiR fsmApi { get; }
        public void PlayAnim(GameRoleEntityR entity, string animName);
        public GameRoleTemplateR GetRoleTemplate();

        /// <summary>
        /// 根据实体Id查找角色
        /// <para>entityId: 实体Id</para>
        /// </summary>
        public GameRoleEntityR FindByEntityId(int entityId);
    }

}
