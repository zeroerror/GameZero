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

        /// <summary> 数值B </summary>
        public readonly int valueB;
        /// <summary> 数值格式B </summary>
        public readonly GameActionValueFormat valueFormatB;
        /// <summary> 数值参考类型A </summary>
        public readonly GameActionValueRefType refTypeB;

        /// <summary> 比较类型 </summary>
        public readonly GameNumCompareType compareType;

        public GameBuffConditionModel_WhenAttributeChange(
            int valueA, GameActionValueFormat valueFormatA, GameActionValueRefType refTypeA,
            int valueB, GameActionValueFormat valueFormatB, GameActionValueRefType refTypeB,
            GameNumCompareType compareType
        )
        {
            this.valueA = valueA;
            this.valueFormatA = valueFormatA;
            this.refTypeA = refTypeA;

            this.valueB = valueB;
            this.valueFormatB = valueFormatB;
            this.refTypeB = refTypeB;

            this.compareType = compareType;
        }
    }
}