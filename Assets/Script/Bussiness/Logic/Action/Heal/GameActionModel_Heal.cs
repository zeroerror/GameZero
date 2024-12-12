namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_Heal : GameActionModelBase
    {
        /// <summary> 治疗类型 </summary>
        public GameActionHealType healType;

        /// <summary> 治疗数值 </summary>
        public int value;
        /// <summary> 数值格式 </summary>
        public GameActionValueFormat valueFormat;
        /// <summary> 数值参考类型 </summary>
        public GameActionValueRefType refType;

        public GameActionModel_Heal(
            int typeId,
            GameEntitySelector selector,
            GameActionHealType healType,
            int value,
            GameActionValueFormat valueFormat,
            GameActionValueRefType refType
        ) : base(GameActionType.Heal, typeId, selector)
        {
            this.healType = healType;
            this.value = value;
            this.valueFormat = valueFormat;
            this.refType = refType;
        }

        public override string ToString()
        {
            return $"治疗类型:{healType}, 治疗数值:{value}, 数值格式:{valueFormat}, 数值参考类型:{refType}";
        }
    }
}