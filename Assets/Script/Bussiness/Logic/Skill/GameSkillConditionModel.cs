namespace GamePlay.Bussiness.Logic
{
    public class GameSkillConditionModel
    {
        public readonly GameSkillTargterType targeterType;
        public readonly float cdTime;
        public readonly float mpCost;
        public readonly GameEntitySelector selector;

        public GameSkillConditionModel(
            GameSkillTargterType targeterType,
            float cdTime,
            float mpCost,
            GameEntitySelector selector
        )
        {
            this.targeterType = targeterType;
            this.cdTime = cdTime;
            this.mpCost = mpCost;
            this.selector = selector;
        }
    }
}