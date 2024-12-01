namespace GamePlay.Bussiness.Renderer
{

    public abstract class GameRoleStateDomainBaseR
    {
        protected GameContextR _context;
        protected GameRoleContextR _roleContext => this._context.roleContext;

        public GameRoleStateDomainBaseR() { }

        public void Inject(GameContextR context)
        {
            this._context = context;
        }

        public virtual void BindEvents() { }

        public virtual void UnbindEvents() { }

        /** 状态更新 */
        public void Tick(GameRoleEntityR entity, float frameTime)
        {
            this._Tick(entity, frameTime);
        }

        /** 进入. ps: 直接调用则会跳过了条件判定 */
        public abstract void Enter(GameRoleEntityR entity, params object[] args);
        /** 状态更新 */
        protected abstract void _Tick(GameRoleEntityR entity, float frameTime);
        /** 退出状态 */
        public virtual void ExitTo(GameRoleEntityR entity, GameRoleStateType toState) { }
    }

}


