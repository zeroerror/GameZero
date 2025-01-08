using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_AttributeModify : GameActionModelBase
    {
        /// <summary> 修改属性 </summary>
        public GameAttributeType modifyType;

        /// <summary> 数值 </summary>
        public int value;
        /// <summary> 数值格式 </summary>
        public GameActionValueFormat valueFormat;
        /// <summary> 数值参考类型 </summary>
        public GameActionValueRefType refType;

        public GameActionModel_AttributeModify(
            int typeId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            in GameVec2 randomValueOffset,
            GameAttributeType attributeType,
            int value,
            GameActionValueFormat valueFormat,
            GameActionValueRefType refType
        ) : base(GameActionType.AttributeModify, typeId, selector, preconditionSet, randomValueOffset)
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

        public override GameActionModelBase GetCustomModel(float customParam)
        {
            // 属性修改数值 = 自定义参数 * 原数值
            var value = GameMath.Floor(customParam * this.value);
            return new GameActionModel_AttributeModify(
                typeId,
                selector,
                preconditionSet,
                randomValueOffset,
                modifyType,
                value,
                valueFormat,
                refType
            );
        }
    }
}