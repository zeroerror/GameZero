namespace GamePlay.Bussiness.Renderer
{
    public class GameDomainApiR
    {
        public GameRoleDomainApiR roleApi { get; private set; }
        public void SetRoleApi(GameRoleDomainApiR roleApi) => this.roleApi = roleApi;
    }
}