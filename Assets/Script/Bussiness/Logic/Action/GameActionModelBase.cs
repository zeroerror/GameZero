namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public abstract class GameActionModelBase
    {
        public int typeId;
        public GameActionType actionType;
        public GameEntitySelector selector;

        public GameActionModelBase(GameActionType actionType, int typeId, GameEntitySelector selector)
        {
            this.actionType = actionType;
            this.typeId = typeId;
            this.selector = selector;
        }
    }
}