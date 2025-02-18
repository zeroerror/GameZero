using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Render
{
    public class GameSkillEntityR : GameEntityBase
    {
        public GameSkillModelR model { get; private set; }

        public GameSkillEntityR(GameSkillModelR skillModel) : base(skillModel.typeId, GameEntityType.Skill)
        {
            this.model = skillModel;
        }

        public override void Tick(float dt)
        {
        }

        public override void Destroy()
        {
        }
    }
}