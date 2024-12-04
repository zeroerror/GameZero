namespace GamePlay.Bussiness.Logic
{
    public class GameDomainApi
    {
        public GameRoleDomainApi roleApi { get; private set; }
        public void SetRoleApi(GameRoleDomainApi roleApi) => this.roleApi = roleApi;

        public GameTransformDomainApi transformApi { get; private set; }
        public void SetTransformApi(GameTransformDomainApi transformApi) => this.transformApi = transformApi;

        public GamePhysicsDomainApi physicsApi { get; private set; }
        public void SetPhysicsApi(GamePhysicsDomainApi physicsApi) => this.physicsApi = physicsApi;

        public GameSkillDomainApi skillApi { get; private set; }
        public void SetSkillApi(GameSkillDomainApi skillApi) => this.skillApi = skillApi;

        public GameActionDomainApi actionApi { get; private set; }
        public void SetActionApi(GameActionDomainApi actionApi) => this.actionApi = actionApi;

        public GameEntitySelectDomainApi entitySelectApi { get; private set; }
        public void SetEntitySelectApi(GameEntitySelectDomainApi entitySelectApi) => this.entitySelectApi = entitySelectApi;

        public GameProjectileDomainApi projectileApi { get; private set; }
        public void SetProjectileApi(GameProjectileDomainApi projectileApi) => this.projectileApi = projectileApi;

        public GameFieldDomainApi fieldApi { get; private set; }
        public void SetFieldApi(GameFieldDomainApi fieldApi) => this.fieldApi = fieldApi;

        public GameEntityCollectDomainApi entityCollectApi { get; private set; }
        public void SetEntityCollectApi(GameEntityCollectDomainApi entityCollectApi) => this.entityCollectApi = entityCollectApi;
    }
}