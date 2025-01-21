using GamePlay.Bussiness.Logic;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_AttributeModify
    {
        public GameAttributeType modifyType;
        public int value;
        public GameActionValueFormat valueFormat;
        public GameActionValueRefType refType;

        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;
        public GameVec2 randomValueOffset;

        public GameActionModel_AttributeModify ToModel()
        {
            return new GameActionModel_AttributeModify(
                0,
                this.selectorEM?.ToModel(),
                this.preconditionSetEM?.ToModel(),
                this.randomValueOffset,
                this.modifyType,
                this.value,
                this.valueFormat,
                this.refType
            );
        }
    }
}