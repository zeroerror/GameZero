using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileFSMDomain : GameProjectileFSMDomainApiR
    {
        GameContextR _context;
        GameProjectileContextR _projectileContext => _context.projectileContext;

        public GameProjectileFSMDomain()
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

        public void Enter(GameProjectileEntityR role, GameProjectileStateType state)
        {
        }
    }
}