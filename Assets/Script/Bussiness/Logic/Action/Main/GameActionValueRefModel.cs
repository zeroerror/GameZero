namespace GamePlay.Bussiness.Logic
{
    public class GameActionValueRefModel
    {
        public readonly int value;
        public readonly GameActionValueFormat valueFormat;
        public readonly GameActionValueRefType refType;

        public GameActionValueRefModel(int value, GameActionValueFormat valueFormat, GameActionValueRefType refType)
        {
            this.value = value;
            this.valueFormat = valueFormat;
            this.refType = refType;
        }
    }
}