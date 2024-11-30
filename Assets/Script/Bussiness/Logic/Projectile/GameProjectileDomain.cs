namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileDomain : GameProjectileDomainApi
    {
        GameContext _context;
        GameProjectileContext _projectileContext => this._context.projectileContext;

        public GameProjectileFSMDomain fsmDomain { get; private set; }
        public GameProjectileFSMDomainApi fsmApi => this.fsmDomain;

        public GameProjectileDomain()
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

        public void Tick(float dt)
        {
        }

    }
}