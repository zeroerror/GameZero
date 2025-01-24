namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public class GameBuffAttributeEM
    {
        public GameAttributeType attributeType;
        public int value;
        public GameActionValueFormat valueFormat;
        public GameActionValueRefType refType;

        public GameBuffAttributeModel ToModel()
        {
            var model = new GameBuffAttributeModel(
                attributeType,
                value,
                valueFormat,
                refType
            );
            return model;
        }
    }
}