namespace GamePlay.Bussiness.Logic
{
    public class GameSkillConditionModel
    {
        public readonly GameSkillTargterType targeterType;
        public readonly float cdTime;

        public GameSkillConditionModel(GameSkillTargterType targeterType, float cdTime)
        {
            this.targeterType = targeterType;
            this.cdTime = cdTime;
        }
    }
}