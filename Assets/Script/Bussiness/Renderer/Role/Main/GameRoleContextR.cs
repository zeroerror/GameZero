
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleContextR
    {
        public GameRoleRepoR repo { get; private set; }
        public GameRoleRepoR transfromRepo { get; private set; }

        public GameRoleFactoryR factory => this._factory;
        GameRoleFactoryR _factory;

        public GameRoleEntityR userRole;

        public GameRoleContextR()
        {
            this.repo = new GameRoleRepoR();
            this.transfromRepo = new GameRoleRepoR();
            this._factory = new GameRoleFactoryR();
        }

    }
}