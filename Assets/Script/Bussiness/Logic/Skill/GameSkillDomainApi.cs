using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public interface GameSkillDomainApi
    {
        public GameSkillEntity CreateSkill(GameRoleEntity role, int typeId);
        public GameSkillModel GetSkillModel(int typeId);
    }
}
