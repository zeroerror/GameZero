namespace GamePlay.Bussiness.Renderer
{
    public class GameDomainApiR
    {
        public GameRoleDomainApiR roleApi { get; private set; }
        public void SetRoleApi(GameRoleDomainApiR roleApi) => this.roleApi = roleApi;

        public GameSkillDomainApiR skillApi { get; private set; }
        public void SetSkillApi(GameSkillDomainApiR skillApi) => this.skillApi = skillApi;

        public GameTransformDomainApiR transformApi { get; private set; }
        public void SetTransformApi(TransformDomainR transformApi) => this.transformApi = transformApi;

        public GameVFXDomainApiR vfxApi { get; private set; }
        public void SetVFXApi(GameVFXDomainApiR vfxApi) => this.vfxApi = vfxApi;
    }
}