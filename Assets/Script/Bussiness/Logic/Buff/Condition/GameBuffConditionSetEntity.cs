namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionSetEntity
    {
        public GameBuffConditionEntity_Duration durationEntity { get; private set; }
        public GameBuffConditionEntity_TimeInterval timeIntervalEntity { get; private set; }

        public GameBuffConditionSetEntity(GameBuffConditionSetModel model)
        {
            if (model.durationModel != null) this.durationEntity = new GameBuffConditionEntity_Duration(model.durationModel);
            if (model.timeIntervalModel != null) this.timeIntervalEntity = new GameBuffConditionEntity_TimeInterval(model.timeIntervalModel);
        }

        public void Clear()
        {
            if (!!this.durationEntity) this.durationEntity.Clear();
            if (!!this.timeIntervalEntity) this.timeIntervalEntity.Clear();
        }

        public void Tick(float dt)
        {
            if (!!this.durationEntity) this.durationEntity.Tick(dt);
            if (!!this.timeIntervalEntity) this.timeIntervalEntity.Tick(dt);
        }

        public bool CheckSatisfied()
        {
            var isSatisfied = true;
            if (!!this.durationEntity && !this.durationEntity.isSatisfied) isSatisfied = false;
            if (!!this.timeIntervalEntity && !this.timeIntervalEntity.isSatisfied) isSatisfied = false;
            return isSatisfied;
        }

        public void RefreshTime()
        {
            if (!this.durationEntity) return;
            this.durationEntity.Clear();
        }

        public void StackTime()
        {
            if (!this.durationEntity) return;
            this.durationEntity.extraStackCount += 1;
        }
    }
}