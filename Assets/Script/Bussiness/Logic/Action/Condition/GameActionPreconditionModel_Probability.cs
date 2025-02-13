using GamePlay.Bussiness.Core;

namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 行为前置条件-概率条件
    /// </summary>
    public class GameActionPreconditionModel_Probability
    {
        public readonly float probability;

        public GameActionPreconditionModel_Probability(float probability)
        {
            this.probability = probability;
        }

        public bool CheckSatisfied()
        {
            var random = GameRandomService.GetRandom(0f, 1f);
            return random <= this.probability;
        }
    }
}