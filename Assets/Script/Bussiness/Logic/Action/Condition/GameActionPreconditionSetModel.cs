using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionPreconditionSetModel
    {
        public readonly GameActionPreconditionModel_Buff buffConditionModel;
        public readonly GameActionPreconditionModel_Probability probabilityConditionModel;

        public GameActionPreconditionSetModel(
            GameActionPreconditionModel_Buff buffConditionModel,
            GameActionPreconditionModel_Probability probabilityConditionModel)
        {
            this.buffConditionModel = buffConditionModel;
            this.probabilityConditionModel = probabilityConditionModel;
        }

        public bool CheckSatisfied(GameEntityBase target)
        {
            if (buffConditionModel != null && !buffConditionModel.CheckSatisfied(target)) return false;
            if (probabilityConditionModel != null && !probabilityConditionModel.CheckSatisfied()) return false;
            return true;
        }
    }
}