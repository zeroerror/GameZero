using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Config
{
    [System.Serializable]
    public class GameActionValueRefEM
    {
        public int value;
        public GameActionValueFormat valueFormat;
        public GameActionValueRefType refType;

        public GameActionValueRefModel ToModel()
        {
            if (this.refType == GameActionValueRefType.None)
            {
                return null;
            }
            return new GameActionValueRefModel(this.value, this.valueFormat, this.refType);
        }
    }
}