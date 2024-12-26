using GamePlay.Bussiness.Logic;
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

        public GameActionModel_AttributeModify ToModel()
        {
            return new GameActionModel_AttributeModify(
                0,
                this.selectorEM?.ToSelector(),
                this.preconditionSetEM?.ToModel(),
                this.modifyType,
                this.value,
                this.valueFormat,
                this.refType
            );
        }
    }
}