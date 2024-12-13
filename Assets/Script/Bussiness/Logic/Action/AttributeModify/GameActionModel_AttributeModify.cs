namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_Attribute : GameActionModelBase
    {
        /// <summary> 修改属性 </summary>
        public GameAttributeType modifyType;

        /// <summary> 数值 </summary>
        public int value;
        /// <summary> 数值格式 </summary>
        public GameActionValueFormat valueFormat;
        /// <summary> 数值参考类型 </summary>
        public GameActionValueRefType refType;

        public GameActionModel_Attribute(
            int typeId,
            GameEntitySelector selector,
            GameAttributeType attributeType,
            int value,
            GameActionValueFormat valueFormat,
            GameActionValueRefType refType
        ) : base(GameActionType.AttributeModify, typeId, selector)
        {
            this.modifyType = attributeType;
            this.value = value;
            this.valueFormat = valueFormat;
            this.refType = refType;
        }

        public override string ToString()
        {
            return $"修改属性:{modifyType}, 数值:{value}, 数值格式:{valueFormat}, 数值参考类型:{refType}";
        }
    }
}