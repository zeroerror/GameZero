using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameSkillEntity : GameEntityBase
    {
        public GameSkillModel skillModel { get; private set; }
        public GameTimelineCom timelineCom { get; private set; }
        public float cdElapsed;

        public GameSkillEntity(GameSkillModel skillModel) : base(skillModel.typeId, GameEntityType.Skill)
        {
            this.skillModel = skillModel;
            this.timelineCom = new GameTimelineCom(skillModel.length);
        }

        public override void Clear()
        {
            base.Clear();
            this.cdElapsed = 0;
            this.timelineCom.Clear();
        }

        public override void Tick(float dt)
        {
            this.cdElapsed -= dt;
            this.cdElapsed = this.cdElapsed < 0 ? 0 : this.cdElapsed;
        }

        public override void Destroy()
        {
        }
    }
}