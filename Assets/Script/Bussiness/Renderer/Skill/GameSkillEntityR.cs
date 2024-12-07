using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameSkillEntityR : GameEntityBase
    {
        public GameSkillModelR skillModel { get; private set; }

        public GameSkillEntityR(GameSkillModelR skillModel) : base(skillModel.typeId, GameEntityType.Skill)
        {
            this.skillModel = skillModel;
        }

        public override void Tick(float dt)
        {
        }

        public override void Destroy()
        {
        }
    }
}