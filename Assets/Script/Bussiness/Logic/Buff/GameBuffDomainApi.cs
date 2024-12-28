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
        public bool AttachBuff(int typeId, GameEntityBase actor, GameEntityBase target, int layer, out int realAttachLayer);

        /// <summary>
        /// 移除buff
        /// <para>target: 目标</para>
        /// <para>buffId: Buff的类型Id</para>
        /// <para>layer: 层数</para>
        /// <para>removeBuff: 移除的Buff</para>
        /// <para>detachLayer: 实际移除层数</para>
        /// </summary>
        public bool TryDetachBuff(GameEntityBase target, int buffId, int layer, out GameBuffEntity removeBuff, out int detachLayer);
    }
}