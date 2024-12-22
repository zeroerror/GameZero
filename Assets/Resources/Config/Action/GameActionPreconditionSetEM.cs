using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionPreconditionSetEM
    {
        public GameActionPreconditionEM_Buff buffConditionEM;

        public GameActionPreconditionSetEM(GameActionPreconditionEM_Buff buffConditionMode)
        {
            this.buffConditionEM = buffConditionMode;
        }

        public GameActionPreconditionSetModel ToModel()
        {
            var model = new GameActionPreconditionSetModel(
                buffConditionEM?.ToModel()
            );
            return model;
        }
    }
}