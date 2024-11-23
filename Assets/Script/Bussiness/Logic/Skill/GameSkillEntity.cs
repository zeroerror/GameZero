namespace GamePlay.Bussiness.Logic
{
    public class GameSkillEntity : GameEntityBase
    {
        public GameSkillModel skillModel { get; private set; }

        public GameSkillEntity(GameSkillModel skillModel) : base(skillModel.typeId, GameEntityType.Skill)
        {
            this.skillModel = skillModel;
        }
    }
}