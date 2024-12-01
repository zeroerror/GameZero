using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public interface GameSkillDomainApi
    {
        public GameSkillEntity CreateSkill(GameRoleEntity role, int typeId);
        public GameSkillModel GetSkillModel(int typeId);
        public bool CheckCastCondition(GameRoleEntity role, GameSkillEntity skill);
        public void CastSkill(GameRoleEntity role, GameSkillEntity skill);
    }
}
