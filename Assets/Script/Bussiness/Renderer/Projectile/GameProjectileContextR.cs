using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileContextR
    {
        public GameProjectileRepo repo => this._repo;
        GameProjectileRepo _repo;
        public GameProjectileFactoryR factory => this._factory;
        GameProjectileFactoryR _factory;

        public GameIdService idService { get; private set; }

        public GameProjectileContextR()
        {
            this._repo = new GameProjectileRepo();
            this._factory = new GameProjectileFactoryR();
            this.idService = new GameIdService();
        }
    }
}