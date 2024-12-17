namespace GamePlay.Bussiness.Logic
{
    public class GameBuffModel
    {
        public readonly int typeId;
        public readonly GameBuffRefreshFlag refreshFlag;
        public readonly int[] actionIds;
        public readonly GameBuffConditionSetModel conditionSetModel_action;
        public readonly GameBuffConditionSetModel conditionSetModel_remove;

        public GameBuffModel(
            int typeId,
            GameBuffRefreshFlag refreshFlag,
            int[] actionIds,
            GameBuffConditionSetModel conditionSetModel_action,
            GameBuffConditionSetModel conditionSetModel_remove
        )
        {
            this.typeId = typeId;
            this.refreshFlag = refreshFlag;
            this.actionIds = actionIds;
            this.conditionSetModel_action = conditionSetModel_action;
            this.conditionSetModel_remove = conditionSetModel_remove;
        }
    }
}