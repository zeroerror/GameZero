using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    /// <summary>
    /// 行为前置条件-概率
    /// </summary>
    [System.Serializable]
    public class GameActionPreconditionEM_Probability
    {
        public bool enable;
        public float probability;

        public GameActionPreconditionModel_Probability ToModel()
        {
            if (!enable) return null;
            var model = new GameActionPreconditionModel_Probability(probability);
            return model;
        }
    }
}