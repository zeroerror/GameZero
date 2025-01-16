namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// buff条件模型 - 当单位数量变化时
    /// </summary>
    public class GameBuffConditionModel_WhenUnitCountChange
    {
        /// <summary> 选择器, 用于选择单位 </summary>
        public readonly GameEntitySelector selector;
        /// <summary> 数量条件 </summary>
        public readonly int countCondition;

        public GameBuffConditionModel_WhenUnitCountChange(
            GameEntitySelector selector,
            int countCondition
        )
        {
            this.selector = selector;
            this.countCondition = countCondition;
        }
    }
}