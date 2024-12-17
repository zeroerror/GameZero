namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionSetModel
    {
        public readonly GameBuffConditionModel_Duration durationModel;
        public readonly GameBuffConditionModel_TimeInterval timeIntervalModel;

        public GameBuffConditionSetModel(GameBuffConditionModel_Duration durationModel, GameBuffConditionModel_TimeInterval timeIntervalModel)
        {
            this.durationModel = durationModel;
            this.timeIntervalModel = timeIntervalModel;
        }
    }
}