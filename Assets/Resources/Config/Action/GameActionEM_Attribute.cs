using GamePlay.Bussiness.Logic;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_Attribute
    {

        public GameAttributeType modifyType;
        public int value;
        public GameActionValueFormat valueFormat;
        public GameActionValueRefType refType;

        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;

        public GameActionModel_Attribute ToModel()
        {
            return new GameActionModel_Attribute(
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