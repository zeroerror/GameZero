namespace GamePlay.Bussiness.Logic
{
    public class GameSkillConditionModel
    {
        public readonly GameSkillTargterType targeterType;
        public readonly float cdTime;
        public readonly GameEntitySelector selector;

        public GameSkillConditionModel(GameSkillTargterType targeterType, float cdTime, GameEntitySelector selector)
        {
            this.targeterType = targeterType;
            this.cdTime = cdTime;
            this.selector = selector;
        }
    }
}