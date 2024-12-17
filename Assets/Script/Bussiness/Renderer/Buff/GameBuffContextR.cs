namespace GamePlay.Bussiness.Renderer
{
    public class GameBuffContextR
    {
        public GameBuffRepoR repo => this._repo;
        GameBuffRepoR _repo;
        public GameBuffFactoryR factory => this._factory;
        GameBuffFactoryR _factory;

        public GameBuffContextR()
        {
            this._repo = new GameBuffRepoR();
            this._factory = new GameBuffFactoryR();
        }
    }
}