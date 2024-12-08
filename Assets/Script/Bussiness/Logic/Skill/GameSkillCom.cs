using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameSkillComp
    {
        private List<GameSkillEntity> _skillEntities;

        public GameSkillComp(GameRoleEntity entity)
        {
            _skillEntities = new List<GameSkillEntity>();
        }

        public void Add(GameSkillEntity skill)
        {
            _skillEntities.Add(skill);
        }

        public bool TryGet(int typeId, out GameSkillEntity skill)
        {
            skill = _skillEntities.Find(s => s.skillModel.typeId == typeId);
            return skill != null;
        }

        public void ForeachSkills(System.Action<GameSkillEntity> action)
        {
            _skillEntities.ForEach(action);
        }

        public GameSkillEntity Find(System.Predicate<GameSkillEntity> predicate)
        {
            return _skillEntities.Find(predicate);
        }
    }
}