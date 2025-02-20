using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameBuffConditionEM_WhenAttributeChange
    {
        public bool isEnable;
        public int valueA;
        public GameActionValueFormat valueFormatA;
        public GameActionValueRefType refTypeA;
        public GameEntitySelectorEM selectorEM_A;

        public int valueB;
        public GameActionValueFormat valueFormatB;
        public GameActionValueRefType refTypeB;
        public GameEntitySelectorEM selectorEM_B;

        public GameNumCompareType compareType;

        public GameBuffConditionModel_WhenAttributeChange ToModel()
        {
            if (!isEnable)
            {
                return null;
            }
            return new GameBuffConditionModel_WhenAttributeChange(
                valueA, valueFormatA, refTypeA, selectorEM_A.ToModel(),
                valueB, valueFormatB, refTypeB, selectorEM_B.ToModel(),
                compareType
            );
        }
    }
}