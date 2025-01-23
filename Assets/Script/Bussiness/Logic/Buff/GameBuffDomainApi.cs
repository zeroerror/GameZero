namespace GamePlay.Bussiness.Logic
{
    public interface GameBuffDomainApi
    {
        /// <summary>
        /// 挂载buff 
        /// <para>buffId: Buff的类型Id</para>
        /// <para>actor: 行为者</para>
        /// <para>targetRole: 目标</para>
        /// <para>layer: 层数</para>
        /// <para>realAttachLayer: 实际挂载层数</para>
        /// @return 是否挂载成功
        /// </summary>
        public bool TryAttachBuff(int typeId, GameEntityBase actor, GameEntityBase target, int layer, out int realAttachLayer);

        /// <summary>
        /// 移除buff
        /// <para>target: 目标</para>
        /// <para>buffId: Buff的类型Id</para>
        /// <para>layer: 层数</para>
        /// <para>removeBuff: 移除的Buff</para>
        /// <para>detachLayer: 实际移除层数</para>
        /// </summary>
        public bool TryDetachBuff(GameEntityBase target, int buffId, int layer, out GameBuffEntity removeBuff, out int detachLayer);

        /// <summary>
        /// 移除所有buff
        /// <para>target: 目标</para>
        /// </summary>
        public void DetachAllBuffs(GameEntityBase target);

        /// <summary>
        /// 转移buff组件内容, 包括buff列表, 以及buff作用的目标
        /// <para> 参考的buff组件 </para>
        /// <para> 目标角色 </para>
        /// </summary>
        public void TranserBuffCom(GameBuffCom refBuffCom, GameRoleEntity targetRole);
    }
}