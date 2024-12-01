using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileContext
    {
        public GameProjectileRepo repo => this._repo;
        GameProjectileRepo _repo;
        public GameProjectileFactoryR factory => this._factory;
        GameProjectileFactoryR _factory;

        public GameIdService idService { get; private set; }

        public GameProjectileContext()
        {
            this._repo = new GameProjectileRepo();
            this._factory = new GameProjectileFactoryR();
            this.idService = new GameIdService();
        }
    }
}