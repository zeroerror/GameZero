namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionEntity_Duration : GameBuffConditionEntityBase
    {
        public GameBuffConditionModel_Duration model { get; private set; }

        /// <summary> 额外叠加的次数, 会影响真正持续的时间 </summary>
        public int extraStackCount;

        public float elapsedTime { get; private set; }

        public GameBuffConditionEntity_Duration(GameBuffEntity buff, GameBuffConditionModel_Duration model) : base(buff)
        {
            this.model = model;
        }

        protected override void _Tick(float dt)
        {
            this.elapsedTime += dt;
        }

        protected override bool _Check()
        {
            var duration = this.model.duration + this.extraStackCount * this.model.duration;
            return this.elapsedTime >= duration;
        }

        public override void Clear()
        {
            this.elapsedTime = 0;
            this.extraStackCount = 0;
        }
    }
}