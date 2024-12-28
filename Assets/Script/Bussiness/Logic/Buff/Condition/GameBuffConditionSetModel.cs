namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionSetModel
    {
        public readonly GameBuffConditionModel_Duration durationModel;
        public readonly GameBuffConditionModel_TimeInterval timeIntervalModel;
        public readonly GameBuffConditionModel_WhenDoAction whenDoActionModel;

        public GameBuffConditionSetModel(
            GameBuffConditionModel_Duration durationModel,
            GameBuffConditionModel_TimeInterval timeIntervalModel,
            GameBuffConditionModel_WhenDoAction whenDoActionModel
        )
        {
            this.durationModel = durationModel;
            this.timeIntervalModel = timeIntervalModel;
            this.whenDoActionModel = whenDoActionModel;
        }
    }
}