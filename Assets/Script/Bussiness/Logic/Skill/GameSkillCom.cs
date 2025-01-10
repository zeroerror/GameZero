using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameSkillCom
    {
        private GameRoleEntity _role;
        private List<GameSkillEntity> _skillEntities;

        public GameSkillCom(GameRoleEntity role)
        {
            _role = role;
            _skillEntities = new List<GameSkillEntity>();
        }

        public void Clear()
        {
            _skillEntities.Foreach(s => s.Clear());
            _skillEntities.Clear();
        }

        public void Add(GameSkillEntity skill)
        {
            _skillEntities.Add(skill);
            _skillEntities.Sort((a, b) => a.skillModel.skillType - b.skillModel.skillType);
            _CorrectMP(skill);
        }

        /// <summary> 修正技能消耗的法力值 </summary>
        private void _CorrectMP(GameSkillEntity skill)
        {
            var mpCost = skill.skillModel.conditionModel.mpCost;
            if (mpCost > this._role.attributeCom.GetValue(GameAttributeType.MaxMP))
            {
                var attr = new GameAttribute() { type = GameAttributeType.MaxMP, value = mpCost };
                this._role.attributeCom.SetAttribute(attr);
            }
        }

        public void CorrectMP()
        {
            this.ForeachSkills(skill =>
            {
                this._CorrectMP(skill);
            });
        }

        public void AddList(List<GameSkillEntity> skills)
        {
            _skillEntities.AddRange(skills);
            _skillEntities.Sort((a, b) => a.skillModel.skillType - b.skillModel.skillType);
        }

        public bool TryGet(int typeId, out GameSkillEntity skill)
        {
            skill = _skillEntities.Find(s => s.skillModel.typeId == typeId);
            return skill != null;
        }

        public void ForeachSkills(System.Action<GameSkillEntity> action)
        {
            _skillEntities.Foreach(action);
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