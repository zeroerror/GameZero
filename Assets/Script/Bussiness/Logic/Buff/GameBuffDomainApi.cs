namespace GamePlay.Bussiness.Logic
{
    public interface GameBuffDomainApi
    {
        /// <summary>
        /// 挂载buff 
        /// <para>buffId Buff的类型Id</para>
        /// <para>targetRole 目标角色</para>
        /// @return 是否挂载成功
        /// </summary>
        public bool AttachBuff(int buffId, GameRoleEntity targetRole);

        /// <summary>
        /// 移除buff
        /// <para>buffId Buff的类型Id</para>
        /// <para>targetRole 目标角色</para>
        /// @return 是否移除成功
        /// </summary>
        public bool RemoveBuff(int buffId, GameRoleEntity targetRole);
    }
}