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
    }
}