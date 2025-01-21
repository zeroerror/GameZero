namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionModel_WhenAttributeChange
    {
        /// <summary> 数值A </summary>
        public readonly int valueA;
        /// <summary> 数值格式A </summary>
        public readonly GameActionValueFormat valueFormatA;
        /// <summary> 数值参考类型A </summary>
        public readonly GameActionValueRefType refTypeA;
        /// <summary> 选择器A </summary>
        public readonly GameEntitySelector selectorA;

        /// <summary> 数值B </summary>
        public readonly int valueB;
        /// <summary> 数值格式B </summary>
        public readonly GameActionValueFormat valueFormatB;
        /// <summary> 数值参考类型A </summary>
        public readonly GameActionValueRefType refTypeB;
        /// <summary> 选择器B </summary>
        public readonly GameEntitySelector selectorB;

        /// <summary> 比较类型 </summary>
        public readonly GameNumCompareType compareType;

        public GameBuffConditionModel_WhenAttributeChange(
            int valueA, GameActionValueFormat valueFormatA, GameActionValueRefType refTypeA, GameEntitySelector selectorA,
            int valueB, GameActionValueFormat valueFormatB, GameActionValueRefType refTypeB, GameEntitySelector selectorB,
            GameNumCompareType compareType
        )
        {
            this.valueA = valueA;
            this.valueFormatA = valueFormatA;
            this.refTypeA = refTypeA;
            this.selectorA = selectorA;

            this.valueB = valueB;
            this.valueFormatB = valueFormatB;
            this.refTypeB = refTypeB;
            this.selectorB = selectorB;

            this.compareType = compareType;
        }
    }
}