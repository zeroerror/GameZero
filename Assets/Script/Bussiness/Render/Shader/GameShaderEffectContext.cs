namespace GamePlay.Bussiness.Render
{
    public class GameShaderEffectContext
    {
        public GameShaderEffectRepo repo => this._repo;
        GameShaderEffectRepo _repo;
        public GameShaderEffectFactory factory => this._factory;
        GameShaderEffectFactory _factory;

        public GameShaderEffectContext()
        {
            this._repo = new GameShaderEffectRepo();
            this._factory = new GameShaderEffectFactory();
        }
    }
}