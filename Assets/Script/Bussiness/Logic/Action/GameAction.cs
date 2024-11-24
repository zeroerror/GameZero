namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public abstract class GameAction
    {
        public int typeId;
        public GameActionType actionType;
    }
}