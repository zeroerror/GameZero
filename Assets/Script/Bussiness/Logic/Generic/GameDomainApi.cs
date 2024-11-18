namespace GamePlay.Bussiness.Logic
{
    public class GameDomainApi
    {
        public GameRoleDomainApi roleApi { get; private set; }
        public void SetRoleApi(GameRoleDomainApi roleApi) => this.roleApi = roleApi;
    }
}