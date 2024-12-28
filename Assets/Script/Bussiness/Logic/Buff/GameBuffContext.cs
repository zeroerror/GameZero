using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameBuffContext
    {
        public GameIdService idService { get; private set; }
        public GameBuffRepo repo => this._repo;
        GameBuffRepo _repo;
        public GameBuffFactory factory => this._factory;
        GameBuffFactory _factory;

        public GameBuffContext()
        {
            this.idService = new GameIdService();
            this._repo = new GameBuffRepo();
            this._factory = new GameBuffFactory();
        }
    }
}