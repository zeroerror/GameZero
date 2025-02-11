using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Render
{

    public abstract class GameProjectileStateDomainBaseR
    {
        protected GameContextR _context;
        protected GameProjectileContextR _projectileContext => this._context.projectileContext;

        public GameProjectileStateDomainBaseR() { }

        public void Inject(GameContextR context)
        {
            this._context = context;
        }

        public virtual void BindEvents() { }

        public virtual void UnbindEvents() { }

        /** 状态更新 */
        public void Tick(GameProjectileEntityR entity, float frameTime)
        {
            this._Tick(entity, frameTime);
        }

        /** 进入. ps: 直接调用则会跳过了条件判定 */
        public abstract void Enter(GameProjectileEntityR entity);
        /** 状态更新 */
        protected abstract void _Tick(GameProjectileEntityR entity, float frameTime);
    }

}