namespace GamePlay.Bussiness.Render
{
    public interface GameRoleFSMDomainApiR
    {
        public void Enter(GameRoleEntityR role, GameRoleStateType toState, params object[] args);
    }
}