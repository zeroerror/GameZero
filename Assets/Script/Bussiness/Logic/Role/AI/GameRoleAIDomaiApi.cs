namespace GamePlay.Bussiness.Logic
{
    public interface GameRoleAIDomainApi
    {
        public void TryEnter(GameRoleEntity role, GameRoleAIStateType stateType);
    }
}