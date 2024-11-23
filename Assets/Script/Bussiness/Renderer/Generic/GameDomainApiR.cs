namespace GamePlay.Bussiness.Renderer
{
    public class GameDomainApiR
    {
        public GameRoleDomainApiR roleApi { get; private set; }
        public void SetRoleApi(GameRoleDomainApiR roleApi) => this.roleApi = roleApi;
        public GameTransformDomainApiR transformApi { get; private set; }
        public void SetTransformApi(TransformDomainR transformApi) => this.transformApi = transformApi;
    }
}