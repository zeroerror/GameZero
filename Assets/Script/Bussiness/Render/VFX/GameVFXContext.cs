using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Render
{
    public class GameVFXContext
    {
        public GameVFXRepo repo => this._repo;
        GameVFXRepo _repo;
        public GameVFXFactory factory => this._factory;
        GameVFXFactory _factory;

        public GameIdService entityIdService { get; private set; }

        public GameVFXContext()
        {
            this._repo = new GameVFXRepo();
            this._factory = new GameVFXFactory();
            this.entityIdService = new GameIdService();
        }
    }
}