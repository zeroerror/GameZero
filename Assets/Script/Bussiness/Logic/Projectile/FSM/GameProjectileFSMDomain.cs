namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileFSMDomain : GameProjectileFSMDomainApi
    {
        GameContext _context;
        GameProjectileContext _projectileContext => _context.projectileContext;

        public GameProjectileFSMDomain()
        {
        }

        public void Inject(GameContext context)
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

        public bool TryEnter(GameProjectileEntity role, GameProjectileStateType state)
        {
            throw new System.NotImplementedException();
        }
    }
}