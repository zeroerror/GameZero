namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// 角色状态记录
    /// </summary>
    public class GameRoleStateRecord
    {
        public readonly int entityId;
        public readonly GameRoleStateType stateType;

        public GameRoleStateRecord(int entityId, GameRoleStateType stateType)
        {
            this.entityId = entityId;
            this.stateType = stateType;
        }
    }
}