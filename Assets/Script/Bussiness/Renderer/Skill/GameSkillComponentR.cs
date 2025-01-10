using System.Collections.Generic;

namespace GamePlay.Bussiness.Renderer
{
    public class GameSkillComR
    {
        private List<GameSkillEntityR> _skillEntities;

        public GameSkillComR(GameRoleEntityR entity)
        {
            _skillEntities = new List<GameSkillEntityR>();
        }

        public void Clear()
        {
            _skillEntities.Clear();
        }

        public void Add(GameSkillEntityR skill)
        {
            _skillEntities.Add(skill);
        }

        public bool TryGet(int typeId, out GameSkillEntityR skill)
        {
            skill = _skillEntities.Find(s => s.skillModel.typeId == typeId);
            return skill != null;
        }

        public bool TryGetByIndex(int index, out GameSkillEntityR skill)
        {
            if (index < 0 || index >= _skillEntities.Count)
            {
                skill = null;
                return false;
            }
            skill = _skillEntities[index];
            return true;
        }
    }
}