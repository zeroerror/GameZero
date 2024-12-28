namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 时间间隔条件
    /// </summary>
    public class GameBuffConditionEntity_WhenDoAction : GameBuffConditionEntityBase
    {
        public GameBuffConditionModel_WhenDoAction model { get; private set; }

        public GameBuffConditionEntity_WhenDoAction(GameBuffConditionModel_WhenDoAction model)
        {
            this.model = model;
        }

        protected override void _Tick(float dt)
        {
        }

        protected override bool _Check()
        {
            var isSatisfied = false;
            // 遍历帧行为记录, 检查是否有符合条件的行为
            this.ForEachActionRecord_Dmg((actionRecord) =>
            {
                var actionId = actionRecord.actionId;
                if (actionId == model.targetActionId)
                {
                    isSatisfied = true;
                }
            });
            return isSatisfied;
        }

        public override void Clear()
        {
        }
    }
}