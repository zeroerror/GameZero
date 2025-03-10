namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionSetModel
    {
        public readonly GameBuffConditionModel_Duration durationModel;
        public readonly GameBuffConditionModel_TimeInterval timeIntervalModel;
        public readonly GameBuffConditionModel_WhenDoAction whenDoActionModel;
        public readonly GameBuffConditionModel_WhenRoleStateEnter whenRoleStateEnterModel;
        public readonly GameBuffConditionModel_WhenUnitCountChange whenUnitCountChangeModel;
        public readonly GameBuffConditionModel_WhenAttributeChange whenAttributeChangeModel;

        public GameBuffConditionSetModel(
            GameBuffConditionModel_Duration durationModel,
            GameBuffConditionModel_TimeInterval timeIntervalModel,
            GameBuffConditionModel_WhenDoAction whenDoActionModel,
            GameBuffConditionModel_WhenRoleStateEnter whenRoleStateEnterModel,
            GameBuffConditionModel_WhenUnitCountChange whenUnitCountChangeModel,
            GameBuffConditionModel_WhenAttributeChange whenAttributeChangeModel
        )
        {
            this.durationModel = durationModel;
            this.timeIntervalModel = timeIntervalModel;
            this.whenDoActionModel = whenDoActionModel;
            this.whenRoleStateEnterModel = whenRoleStateEnterModel;
            this.whenUnitCountChangeModel = whenUnitCountChangeModel;
            this.whenAttributeChangeModel = whenAttributeChangeModel;
        }
    }
}