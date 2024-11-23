using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameSkillComponent
    {
        private List<GameSkillEntity> _skillEntities;

        public GameSkillComponent(GameRoleEntity entity)
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
    }
}