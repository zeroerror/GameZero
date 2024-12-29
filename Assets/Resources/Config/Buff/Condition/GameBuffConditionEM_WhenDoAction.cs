using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameBuffConditionEM_WhenDoAction
    {
        public bool isEnable;
        public GameActionSO targetAction;
        public GameActionType targetActionType;

        public GameBuffConditionModel_WhenDoAction ToModel()
        {
            if (!this.isEnable) return null;
            if (this.targetAction == null
                && this.targetActionType == GameActionType.None
            ) return null;

            return new GameBuffConditionModel_WhenDoAction(this.targetAction?.typeId ?? 0, this.targetActionType);
        }
    }
}