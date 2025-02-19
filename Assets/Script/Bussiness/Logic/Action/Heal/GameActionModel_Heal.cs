using GamePlay.Core;
using GamePlay.Infrastructure;
using GameVec2 = UnityEngine.Vector2;

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
            GameActionPreconditionSetModel preconditionSet,
            in GameVec2 randomValueOffset,
            GameActionHealType healType,
            int value,
            GameActionValueFormat valueFormat,
            GameActionValueRefType refType
        ) : base(GameActionType.Heal, typeId, selector, preconditionSet, randomValueOffset)
        {
            this.healType = healType;
            this.value = value;
            this.valueFormat = valueFormat;
            this.refType = refType;
        }

        public override GameActionModelBase GetCustomModel(float customParam)
        {
            // 治疗数值 = 自定义参数 * 原数值
            var value = GameMath.Floor(customParam * this.value);
            return new GameActionModel_Heal(
                typeId,
                selector,
                preconditionSet,
                randomValueOffset,
                healType,
                value,
                valueFormat,
                refType
            );
        }

        public override string ToString()
        {
            return $"治疗类型:{healType}, 治疗数值:{value}, 数值格式:{valueFormat}, 数值参考类型:{refType}";
        }
    }
}