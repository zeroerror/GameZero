using System.Collections.Generic;

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
        /// 执行行为 - 发射投射物
        /// action: 发射投射物行为
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction_LaunchProjectile(GameActionModel_LaunchProjectile action, GameEntityBase actorEntity);

        /// <summary>
        /// 尝试获取行为模型
        /// <para>actionId 行为ID</para>
        /// <para>model 行为模型</para>
        /// </summary>
        public bool TryGetModel(int actionId, out GameActionModelBase model);
    }
}