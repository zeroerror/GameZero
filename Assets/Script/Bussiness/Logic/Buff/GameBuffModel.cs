namespace GamePlay.Bussiness.Logic
{
    public class GameBuffModel
    {
        public readonly int typeId;
        public readonly string desc;
        public readonly GameBuffRefreshFlag refreshFlag;
        public readonly int maxLayer;
        public readonly float actionParam;
        public readonly int[] actionIds;
        public readonly float actionCD;
        public readonly GameBuffConditionSetModel conditionSetModel_action;
        public readonly GameBuffConditionSetModel conditionSetModel_remove;
        public readonly GameBuffAttributeModel[] attributeModels;

        public GameBuffModel(
            int typeId,
            string desc,
            GameBuffRefreshFlag refreshFlag,
            int maxLayer,
            int actionParam,
            int[] actionIds,
            float actionCD,
            GameBuffConditionSetModel conditionSetModel_action,
            GameBuffConditionSetModel conditionSetModel_remove,
            GameBuffAttributeModel[] attributeModels
        )
        {
            this.typeId = typeId;
            this.desc = desc;
            this.refreshFlag = refreshFlag;
            this.maxLayer = maxLayer;
            this.actionParam = actionParam * 0.01f;
            this.actionIds = actionIds;
            this.actionCD = actionCD;
            this.conditionSetModel_action = conditionSetModel_action;
            this.conditionSetModel_remove = conditionSetModel_remove;
            this.attributeModels = attributeModels;
        }

        public override string ToString()
        {
            return $"[{typeId}] 描述:{desc}";
        }
    }
}