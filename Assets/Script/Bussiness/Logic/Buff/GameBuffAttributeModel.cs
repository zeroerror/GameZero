namespace GamePlay.Bussiness.Logic
{
    public class GameBuffAttributeModel
    {
        /// <summary> 修改属性 </summary>
        public GameAttributeType modifyType;

        /// <summary> 数值 </summary>
        public int value;
        /// <summary> 数值格式 </summary>
        public GameActionValueFormat valueFormat;
        /// <summary> 数值参考类型 </summary>
        public GameActionValueRefType refType;

        public GameBuffAttributeModel(
            GameAttributeType attributeType,
            int value,
            GameActionValueFormat valueFormat,
            GameActionValueRefType refType
        )
        {
            this.modifyType = attributeType;
            this.value = value;
            this.valueFormat = valueFormat;
            this.refType = refType;
        }
    }
}