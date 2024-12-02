namespace GamePlay.Bussiness.Logic
{
    public class GameFieldContext
    {
        public GameFieldRepo repo => this._repo;
        GameFieldRepo _repo;
        public GameFieldFactory factory => this._factory;
        GameFieldFactory _factory;

        public GameFieldEntity curField { get; set; }

        public GameFieldContext()
        {
            this._repo = new GameFieldRepo();
            this._factory = new GameFieldFactory();
        }
    }
}