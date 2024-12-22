using GamePlay.Bussiness.Logic;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_Dmg
    {
        public GameActionDmgType dmgType;
        public int value;
        public GameActionValueFormat valueFormat;
        public GameActionValueRefType refType;

        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;

        public GameActionModel_Dmg ToModel()
        {
            return new GameActionModel_Dmg(
                0,
                this.selectorEM?.ToSelector(),
                this.preconditionSetEM?.ToModel(),
                this.dmgType,
                this.value,
                this.valueFormat,
                this.refType
            );
        }
    }
}