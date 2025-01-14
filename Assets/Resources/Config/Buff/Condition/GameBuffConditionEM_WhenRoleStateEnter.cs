using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameBuffConditionEM_WhenRoleStateEnter
    {
        public bool isEnable;
        public GameCampType campType;
        public GameRoleStateType stateType;
        public GameSkillType skillType;

        public GameBuffConditionModel_WhenRoleStateEnter ToModel()
        {
            if (!this.isEnable) return null;
            return new GameBuffConditionModel_WhenRoleStateEnter(this.campType, this.stateType, this.skillType);
        }
    }
}