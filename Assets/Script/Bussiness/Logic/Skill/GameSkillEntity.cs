namespace GamePlay.Bussiness.Logic
{
    public class GameSkillEntity : GameEntityBase
    {
        public GameSkillModel skillModel { get; private set; }

        public GameTimelineCom timelineCom { get; private set; }
        public GameSkillDashCom dashCom { get; private set; }

        public float cdElapsed;

        public GameSkillEntity(GameSkillModel skillModel) : base(skillModel.typeId, GameEntityType.Skill)
        {
            this.skillModel = skillModel;
            this.SetByModel(skillModel);
        }

        public void SetByModel(GameSkillModel model)
        {
            this.timelineCom = new GameTimelineCom(model.length);
            this.dashCom = new GameSkillDashCom(model.movementModel);
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

        public bool CanTickTimeline()
        {
            var movementModel = skillModel.movementModel;
            var movementType = movementModel.movementType;
            var isDash = movementType == GameSkillMovementType.FixedTimeDash || movementType == GameSkillMovementType.FixedSpeedDash;
            if (!isDash)
            {
                return true;
            }

            var isDashing = this.IsDashing();
            if (!isDashing)
            {
                return true;
            }
            return !movementModel.pauseTimeline;
        }

        public bool IsDashing()
        {
            var curFrame = this.timelineCom.frame;
            return this.dashCom.dashBeginFrame <= curFrame && curFrame < this.dashCom.dashEndFrame;
        }
    }
}