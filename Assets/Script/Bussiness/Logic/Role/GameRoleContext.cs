
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleContext
    {
        public GameRoleRepo repo => this._repo;
        GameRoleRepo _repo;
        public GameRoleFactory factory => this._factory;
        GameRoleFactory _factory;

        public GameRoleContext()
        {
            this._repo = new GameRoleRepo();
            this._factory = new GameRoleFactory();
        }

    }
}