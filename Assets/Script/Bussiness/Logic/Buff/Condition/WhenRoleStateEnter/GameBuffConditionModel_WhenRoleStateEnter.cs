namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// buff条件模型 - 当角色状态进入时
    /// </summary>
    public class GameBuffConditionModel_WhenRoleStateEnter
    {
        /// <summary> 阵营类型(None代表自身) </summary>
        public readonly GameCampType campType;
        /// <summary> 角色状态类型 </summary>
        public readonly GameRoleStateType stateType;
        /// <summary> 技能类型(作为施法状态的进一步筛选) </summary>
        public readonly GameSkillType skillType;

        public GameBuffConditionModel_WhenRoleStateEnter(
            GameCampType campType,
            GameRoleStateType stateType,
            GameSkillType skillType
        )
        {
            this.campType = campType;
            this.stateType = stateType;
            this.skillType = skillType;
        }
    }
}