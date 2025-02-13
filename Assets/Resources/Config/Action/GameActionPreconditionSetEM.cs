using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionPreconditionSetEM
    {
        public GameActionPreconditionEM_Buff buffConditionEM;
        public GameActionPreconditionEM_Probability probabilityConditionEM;

        public GameActionPreconditionSetEM(
            GameActionPreconditionEM_Buff buffConditionModel,
            GameActionPreconditionEM_Probability probabilityConditionModel)
        {
            this.buffConditionEM = buffConditionModel;
            this.probabilityConditionEM = probabilityConditionModel;
        }

        public GameActionPreconditionSetModel ToModel()
        {
            var model = new GameActionPreconditionSetModel(
                buffConditionEM?.ToModel(),
                probabilityConditionEM?.ToModel()
            );
            return model;
        }
    }
}