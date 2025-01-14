namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 角色状态记录
    /// </summary>
    public class GameRoleStateRecord
    {
        public readonly int entityId;
        public readonly GameRoleStateType stateType;
        public readonly GameSkillType skillType;

        public GameRoleStateRecord(int entityId, GameRoleStateType stateType, GameSkillType skillType)
        {
            this.entityId = entityId;
            this.stateType = stateType;
            this.skillType = skillType;
        }
    }
}