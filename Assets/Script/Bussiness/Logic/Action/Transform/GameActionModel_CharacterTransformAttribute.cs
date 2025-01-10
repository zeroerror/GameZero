namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public class GameActionModel_CharacterTransformAttribute
    {
        /// <summary> 属性 </summary>
        public GameAttributeType attributeType;
        /// <summary> 数值 </summary>
        public int value;
        /// <summary> 数值格式 </summary>
        public GameActionValueFormat valueFormat;
        /// <summary> 数值参考类型 </summary>
        public GameActionValueRefType refType;

        public GameActionModel_CharacterTransformAttribute(
            GameAttributeType attributeType,
            int value,
            GameActionValueFormat valueFormat,
            GameActionValueRefType refType
        )
        {
            this.attributeType = attributeType;
            this.value = value;
            this.valueFormat = valueFormat;
            this.refType = refType;
        }

        public override string ToString()
        {
            return $"属性:{attributeType}, 数值:{value}, 数值格式:{valueFormat}, 数值参考类型:{refType}";
        }

        public GameActionModel_CharacterTransformAttribute Clone()
        {
            return new GameActionModel_CharacterTransformAttribute(this.attributeType, this.value, this.valueFormat, this.refType);
        }
    }
}