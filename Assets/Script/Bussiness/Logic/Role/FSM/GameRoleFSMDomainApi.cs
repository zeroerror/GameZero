namespace GamePlay.Bussiness.Logic
{
    public interface GameRoleFSMDomainApi
    {
        public void Enter(GameRoleEntity role, GameRoleStateType state, params object[] args);

    }
}
