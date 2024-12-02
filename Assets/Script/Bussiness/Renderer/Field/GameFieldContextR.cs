namespace GamePlay.Bussiness.Renderer
{
    public class GameFieldContextR
    {
        public GameFieldRepoR repo => this._repo;
        GameFieldRepoR _repo;
        public GameFieldFactoryR factory => this._factory;
        GameFieldFactoryR _factory;

        public GameFieldEntityR curField { get; set; }

        public GameFieldContextR()
        {
            this._repo = new GameFieldRepoR();
            this._factory = new GameFieldFactoryR();
        }
    }
}