using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public interface GameSkillDomainApi
    {
        public GameSkillEntity CreateSkill(GameRoleEntity role, int typeId);
        public GameSkillModel GetSkillModel(int typeId);
        public bool CheckCastCondition(GameRoleEntity role, GameSkillEntity skill);
        public void CastSkill(GameRoleEntity role, GameSkillEntity skill);


        /// <summary>
        /// 尝试获取技能模型
        /// <para>typeId 类型Id</para>
        /// <para>model 技能模型</para>
        /// </summary>
        public bool TryGetModel(int typeId, out GameSkillModel model);
    }
}
