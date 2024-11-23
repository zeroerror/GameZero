namespace GamePlay.Bussiness.Logic
{
    public interface GameRoleFSMDomainApi
    {
        public bool TryEnter(GameRoleEntity role, GameRoleStateType state);

    }
}
