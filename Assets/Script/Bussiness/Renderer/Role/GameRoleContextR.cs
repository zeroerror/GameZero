
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleContextR
    {
        public GameRoleRepoR repo => this._repo;
        GameRoleRepoR _repo;
        public GameRoleFactoryR factory => this._factory;
        GameRoleFactoryR _factory;

        public GameContextR context { get; private set; }

        public GameRoleContextR(GameContextR context)
        {
            this.context = context;
            this._repo = new GameRoleRepoR();
            this._factory = new GameRoleFactoryR(context);
        }

    }
}