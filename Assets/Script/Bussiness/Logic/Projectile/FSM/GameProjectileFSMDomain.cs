using System.Runtime.CompilerServices;

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

        public bool TryEnter(GameProjectileEntity entity, GameProjectileStateType state)
        {
            throw new System.NotImplementedException();
        }

        public void InitCondition(GameProjectileEntity entity)
        {
            var model = entity.model;
            var dict = model.stateTriggerDict;
            foreach (var pair in dict)
            {
                var stateType = pair.Key;
                var actionList = pair.Value;
            }
        }
    }
}