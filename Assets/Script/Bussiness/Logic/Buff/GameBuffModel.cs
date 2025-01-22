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

        /// <summary> 用于根据当前满足选择的目标数量决定buff层数 </summary>
        public readonly GameEntitySelector layerSelector;
        /// <summary> 用于根据引用的属性值决定buff层数 </summary>
        public readonly GameActionValueRefModel layerValueRefModel;

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
            GameBuffAttributeModel[] attributeModels,
            GameEntitySelector layerSelector,
            in GameActionValueRefModel layerValueRefModel
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
            this.layerSelector = layerSelector;
            this.layerValueRefModel = layerValueRefModel;
        }

        public override string ToString()
        {
            return $"[{typeId}] 描述:{desc}";
        }

        /// <summary>
        /// 是否需要在0层时移除
        /// <para> 存在层数监控时不移除 </para>
        /// </summary>
        public bool NeedRemoveAtZeroLayer()
        {
            if (this.HasLayerMonitor()) return false;
            return true;
        }

        /// <summary>
        /// 是否有层数监控
        /// </summary>
        public bool HasLayerMonitor()
        {
            return this.layerSelector != null || this.layerValueRefModel != null;
        }
    }
}