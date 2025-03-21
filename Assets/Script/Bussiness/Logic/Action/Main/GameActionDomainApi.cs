using System.Collections.Generic;

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
        /// 获取行为选项模型列表
        /// </summary>
        public List<GameActionOptionModel> GetActionOptionModelList();

        /// <summary>
        /// 执行行为 
        /// <para>actionId 行为ID</para>
        /// <para>actor 执行者</para>
        /// </summary>
        public void DoAction(int actionId, GameEntityBase actor);

        /// <summary>
        /// 执行行为
        /// <para>actionId 行为ID</para>
        /// <para>actor 执行者</para>
        /// <para>customParam 自定义参数</para>
        public void DoAction(int actionId, GameEntityBase actor, float customParam);

        /// <summary>
        /// 执行行为选项
        /// <para>optionId 选项ID</para>
        /// <para>campId 阵营ID</para>
        /// </summary>
        public void DoActionOption(int optionId, int campId);

        /// <summary>
        /// 执行行为 - 伤害
        /// <para>action 伤害行为</para>
        /// <para>actor 执行者</para>
        /// </summary>
        public void DoAction_Dmg(GameActionModel_Dmg action, GameEntityBase actor);

        /// <summary>
        /// 执行行为 - 治疗
        /// <para>action 治疗行为</para>
        /// <para>actor 执行者</para>
        /// </summary>
        public void DoAction_Heal(GameActionModel_Heal action, GameEntityBase actor);

        /// <summary>
        /// 执行行为 - 发射投射物
        /// action: 发射投射物行为
        /// actor: 执行者
        /// </summary>
        public void DoAction_LaunchProjectile(GameActionModel_LaunchProjectile action, GameEntityBase actor);

        /// <summary>
        /// 执行行为 - 击退
        /// action: 击退行为
        /// actor: 执行者
        /// </summary>
        public void DoAction_KnockBack(GameActionModel_KnockBack action, GameEntityBase actor);

        /// <summary>
        /// 执行行为 - 属性修改
        /// action: 属性修改行为
        /// actor: 执行者
        /// </summary>
        public void DoAction_AttributeModify(GameActionModel_AttributeModify action, GameEntityBase actor, bool dontDo = false);

        /// <summary>
        /// 执行行为 - 附加buff
        /// action: 附加buff行为
        /// actor: 执行者
        /// </summary>
        public void DoAction_AttachBuff(GameActionModel_AttachBuff action, GameEntityBase actor);

        /// <summary>
        /// 执行行为 - 召唤友军 
        /// action: 召唤友军行为
        /// actor: 执行者
        /// </summary>
        public void DoAction_SummonRole(GameActionModel_SummonRoles action, GameEntityBase actor);

        /// <summary>
        /// 执行行为 - 变身
        /// action: 变身行为
        /// actor: 执行者
        /// </summary>
        public void DoAction_CharacterTransform(GameActionModel_CharacterTransform action, GameEntityBase actor);

        /// <summary>
        /// 执行行为 - 隐身
        /// action: 隐身行为
        /// actor: 执行者
        /// </summary>
        public void DoAction_Stealth(GameActionModel_Stealth action, GameEntityBase actor);
    }
}