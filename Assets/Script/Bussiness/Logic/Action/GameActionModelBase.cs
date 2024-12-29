namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public abstract class GameActionModelBase
    {
        public int typeId;
        public GameActionType actionType;
        public GameEntitySelector selector;
        public GameActionPreconditionSetModel preconditionSet;

        public GameActionModelBase(
            GameActionType actionType,
            int typeId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet
        )
        {
            this.actionType = actionType;
            this.typeId = typeId;
            this.selector = selector;
            this.preconditionSet = preconditionSet;
        }

        public abstract GameActionModelBase GetCustomModel(int customParam);
    }
}