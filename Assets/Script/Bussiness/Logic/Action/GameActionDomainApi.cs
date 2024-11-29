namespace GamePlay.Bussiness.Logic
{
    public interface GameActionDomainApi
    {

        /// <summary>
        /// 执行行为 
        /// <para>actionId 行为ID</para>
        /// <para>actorEntity 执行者</para>
        /// </summary>
        public void DoAction(int actionId, GameEntityBase actorEntity);

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
        /// 执行行为 - 发射弹体
        /// action: 发射弹体行为
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction_LaunchProjectile(GameActionModel_LaunchProjectile action, GameEntityBase actorEntity);
    }
}