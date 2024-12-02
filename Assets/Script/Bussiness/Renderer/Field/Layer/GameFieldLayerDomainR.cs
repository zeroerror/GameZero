namespace GamePlay.Bussiness.Renderer
{
    public class GameFieldLayerDomainR
    {
        GameContextR _context;

        public GameFieldLayerDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            this._BindEvent();
        }

        public void Dispose()
        {
            this._UnbindEvents();
        }

        private void _BindEvent()
        {
        }

        private void _UnbindEvents()
        {
        }

    }
}