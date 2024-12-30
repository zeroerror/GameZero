namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// buff条件模型 - 当死亡时(即亡语)
    /// </summary>
    public class GameBuffConditionModel_WhenRoleStateEnter
    {
        /// <summary> 阵营类型(None代表自身) </summary>
        public readonly GameCampType campType;
        /// <summary> 角色状态类型 </summary>
        public readonly GameRoleStateType stateType;

        public GameBuffConditionModel_WhenRoleStateEnter(GameCampType campType, GameRoleStateType stateType)
        {
            this.campType = campType;
            this.stateType = stateType;
        }
    }
}