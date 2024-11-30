namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileContext
    {
        public GameProjectileRepo repo => this._repo;
        GameProjectileRepo _repo;
        public GameProjectileFactory factory => this._factory;
        GameProjectileFactory _factory;

        public GameProjectileContext()
        {
            this._repo = new GameProjectileRepo();
            this._factory = new GameProjectileFactory();
        }
    }
}