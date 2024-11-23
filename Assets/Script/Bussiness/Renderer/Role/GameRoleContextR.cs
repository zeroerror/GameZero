
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleContextR
    {
        public GameRoleRepoR repo => this._repo;
        GameRoleRepoR _repo;
        public GameRoleFactoryR factory => this._factory;
        GameRoleFactoryR _factory;

        public GameRoleEntityR userRole;

        public GameRoleContextR()
        {
            this._repo = new GameRoleRepoR();
            this._factory = new GameRoleFactoryR();
        }

    }
}