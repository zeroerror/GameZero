
namespace GamePlay.Bussiness.Render
{
    public class GameRoleContextR
    {
        public GameRoleRepoR repo { get; private set; }
        public GameRoleRepoR transformRepo { get; private set; }

        public GameRoleFactoryR factory => this._factory;
        GameRoleFactoryR _factory;

        public GameRoleEntityR userRole;

        public GameRoleContextR()
        {
            this.repo = new GameRoleRepoR();
            this.transformRepo = new GameRoleRepoR();
            this._factory = new GameRoleFactoryR();
        }

    }
}