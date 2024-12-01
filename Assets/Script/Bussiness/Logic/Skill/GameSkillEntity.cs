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

        public override void Reset()
        {
            base.Reset();
            this.cdElapsed = 0;
            this.timelineCom.Reset();
        }

        public override void Tick(float dt)
        {
        }

        public override void Dispose()
        {
        }
    }
}