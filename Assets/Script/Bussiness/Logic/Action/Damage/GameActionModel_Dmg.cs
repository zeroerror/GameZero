using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_Dmg : GameActionModelBase
    {
        /// <summary> 伤害类型 </summary>
        public GameActionDmgType dmgType;

        /// <summary> 伤害数值 </summary>
        public int value;
        /// <summary> 数值格式 </summary>
        public GameActionValueFormat valueFormat;
        /// <summary> 数值参考类型 </summary>
        public GameActionValueRefType refType;

        public GameActionModel_Dmg(
            int typeId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            GameActionDmgType dmgType,
            int value,
            GameActionValueFormat valueFormat,
            GameActionValueRefType refType
        ) : base(GameActionType.Dmg, typeId, selector, preconditionSet)
        {
            this.dmgType = dmgType;
            this.value = value;
            this.valueFormat = valueFormat;
            this.refType = refType;
        }

        public override GameActionModelBase GetCustomModel(float customParam)
        {
            // 伤害数值 = 自定义参数 * 原数值
            var value = GameMath.Floor(customParam * this.value);
            return new GameActionModel_Dmg(
                typeId,
                selector,
                preconditionSet,
                dmgType,
                value,
                valueFormat,
                refType
            );
        }

        public override string ToString()
        {
            return $"伤害类型:{dmgType}, 伤害数值:{value}, 数值格式:{valueFormat}, 数值参考类型:{refType}";
        }
    }
}