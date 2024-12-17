namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 时间间隔条件
    /// </summary>
    public class GameBuffConditionEntity_TimeInterval : GameBuffConditionEntityBase
    {
        public GameBuffConditionModel_TimeInterval model { get; private set; }

        public float elapsedTime { get; private set; }

        public GameBuffConditionEntity_TimeInterval(GameBuffConditionModel_TimeInterval model)
        {
            this.model = model;
        }

        protected override void _Tick(float dt)
        {
            this.elapsedTime += dt;
        }

        protected override bool _Check()
        {
            if (this.elapsedTime >= this.model.timeInterval)
            {
                this.elapsedTime = 0;
                return true;
            }
            return false;
        }

        public override void Clear()
        {
            this.elapsedTime = 0;
        }
    }
}