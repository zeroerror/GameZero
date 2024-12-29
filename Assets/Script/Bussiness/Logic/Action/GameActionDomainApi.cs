namespace GamePlay.Bussiness.Logic
{
    public interface GameActionDomainApi
    {
        /// <summary>
        /// 尝试获取行为模型
        /// <para>actionId 行为ID</para>
        /// <para>model 行为模型</para>
        /// </summary>
        public bool TryGetModel(int actionId, out GameActionModelBase model);

        /// <summary>
        /// 执行行为 
        /// <para>actionId 行为ID</para>
        /// <para>actorEntity 执行者</para>
        /// </summary>
        public void DoAction(int actionId, GameEntityBase actorEntity);

        /// <summary>
        /// 执行行为
        /// <para>actionId 行为ID</para>
        /// <para>actor 执行者</para>
        /// <para>customParam 自定义参数</para>
        public void DoAction(int actionId, GameEntityBase actor, float customParam);

        /// <summary>
        /// 执行行为 - 伤害
        /// <para>action 伤害行为</para>
        /// <para>actorEntity 执行者</para>
        /// </summary>
        public void DoAction_Dmg(GameActionModel_Dmg action, GameEntityBase actorEntity);

        /// <summary>
        /// 执行行为 - 治疗
        /// <para>action 治疗行为</para>
        /// <para>actorEntity 执行者</para>
        /// </summary>
        public void DoAction_Heal(GameActionModel_Heal action, GameEntityBase actorEntity);

        /// <summary>
        /// 执行行为 - 发射投射物
        /// action: 发射投射物行为
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction_LaunchProjectile(GameActionModel_LaunchProjectile action, GameEntityBase actorEntity);

        /// <summary>
        /// 执行行为 - 击退
        /// action: 击退行为
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction_KnockBack(GameActionModel_KnockBack action, GameEntityBase actorEntity);

        /// <summary>
        /// 执行行为 - 属性修改
        /// action: 属性修改行为
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction_AttributeModify(GameActionModel_AttributeModify action, GameEntityBase actorEntity, bool dontDo = false);

        /// <summary>
        /// 执行行为 - 附加buff
        /// action: 附加buff行为
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction_AttachBuff(GameActionModel_AttachBuff action, GameEntityBase actorEntity);

        /// <summary>
        /// 执行行为 - 召唤友军 
        /// action: 召唤友军行为
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction_SummonRole(GameActionModel_SummonRoles action, GameEntityBase actorEntity);
    }
}