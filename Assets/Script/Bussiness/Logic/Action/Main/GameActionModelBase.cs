using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public abstract class GameActionModelBase
    {
        public int typeId;
        public GameActionType actionType;
        public GameEntitySelector selector;
        public GameActionPreconditionSetModel preconditionSet;
        public GameVec2 randomValueOffset;

        public GameActionModelBase(
            GameActionType actionType,
            int typeId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            in GameVec2 randomValueOffset
        )
        {
            this.actionType = actionType;
            this.typeId = typeId;
            this.selector = selector;
            this.preconditionSet = preconditionSet;
            this.randomValueOffset = randomValueOffset;
        }

        public abstract GameActionModelBase GetCustomModel(float customParam);
    }
}