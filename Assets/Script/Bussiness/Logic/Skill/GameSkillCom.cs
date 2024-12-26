using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameSkillCom
    {
        private List<GameSkillEntity> _skillEntities;

        public GameSkillCom(GameRoleEntity entity)
        {
            _skillEntities = new List<GameSkillEntity>();
        }

        public void Add(GameSkillEntity skill)
        {
            _skillEntities.Add(skill);
            _skillEntities.Sort((a, b) => a.skillModel.skillType - b.skillModel.skillType);
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

        /// <summary>
        /// 普通查找
        /// </summary>
        public GameSkillEntity Find(System.Predicate<GameSkillEntity> predicate)
        {
            return _skillEntities.Find(predicate);
        }

        /// <summary>
        /// 优先查找, 比如法力攻击优先于普通攻击
        /// </summary>
        public GameSkillEntity FindWithPriority(System.Predicate<GameSkillEntity> predicate)
        {
            for (int i = _skillEntities.Count - 1; i >= 0; i--)
            {
                var skill = _skillEntities[i];
                if (predicate(skill))
                {
                    return skill;
                }
            }
            return null;
        }
    }
}