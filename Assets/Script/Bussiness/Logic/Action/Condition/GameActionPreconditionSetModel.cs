namespace GamePlay.Bussiness.Logic
{
    public class GameActionPreconditionSetModel
    {
        public readonly GameActionPreconditionModel_Buff buffConditionModel;

        public GameActionPreconditionSetModel(GameActionPreconditionModel_Buff buffConditionMode)
        {
            this.buffConditionModel = buffConditionMode;
        }

        public bool CheckSatisfied(GameEntityBase target)
        {
            if (buffConditionModel != null && !buffConditionModel.CheckSatisfied(target)) return false;
            return true;
        }
    }
}