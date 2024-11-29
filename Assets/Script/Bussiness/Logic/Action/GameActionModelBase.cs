namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public abstract class GameActionModelBase
    {
        public int typeId;
        public GameActionType actionType;
        public GameEntitySelector selector;
    }
}