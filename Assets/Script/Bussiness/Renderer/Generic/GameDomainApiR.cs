namespace GamePlay.Bussiness.Renderer
{
    public class GameDomainApiR
    {
        public GameDirectorDomainApiR directApi { get; private set; }
        public void SetDirectApi(GameDirectorDomainApiR directApi) => this.directApi = directApi;

        public GameRoleDomainApiR roleApi { get; private set; }
        public void SetRoleApi(GameRoleDomainApiR roleApi) => this.roleApi = roleApi;

        public GameSkillDomainApiR skillApi { get; private set; }
        public void SetSkillApi(GameSkillDomainApiR skillApi) => this.skillApi = skillApi;

        public GameTransformDomainApiR transformApi { get; private set; }
        public void SetTransformApi(GameTransformDomainR transformApi) => this.transformApi = transformApi;

        public GameAttributeDomainApiR attributeApi { get; private set; }
        public void SetAttributeApi(GameAttributeDomainApiR attributeApi) => this.attributeApi = attributeApi;

        public GameVFXDomainApiR vfxApi { get; private set; }
        public void SetVFXApi(GameVFXDomainApiR vfxApi) => this.vfxApi = vfxApi;

        public GameActionDomainApiR actionApi { get; private set; }
        public void SetActionApi(GameActionDomainApiR actionApi) => this.actionApi = actionApi;

        public GameDrawDomainApiR drawApi { get; private set; }
        public void SetDrawApi(GameDrawDomainApiR drawApi) => this.drawApi = drawApi;

        public GameProjectileDomainApiR projectileApi { get; private set; }
        public void SetProjectileApi(GameProjectileDomainApiR projectileApi) => this.projectileApi = projectileApi;

        public GameFieldDomainApiR fielApi { get; private set; }
        public void SetFieldApi(GameFieldDomainApiR fielApi) => this.fielApi = fielApi;

        public GameEntityCollectDomainApiR entityCollectApi { get; private set; }
        public void SetEntityCollectApi(GameEntityCollectDomainApiR entityCollectApi) => this.entityCollectApi = entityCollectApi;

        public GameBuffDomainApiR buffApi { get; private set; }
        public void SetBuffApi(GameBuffDomainApiR buffApi) => this.buffApi = buffApi;

    }
}