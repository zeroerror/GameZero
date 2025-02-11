namespace GamePlay.Bussiness.Render
{

    public abstract class GameRoleStateDomainBaseR
    {
        protected GameContextR _context;
        protected GameRoleContextR _roleContext => this._context.roleContext;

        /// <summary> 切换状态接口 </summary>
        public delegate void TransitToDelegate(GameRoleEntityR role, GameRoleStateType state, params object[] args);
        protected readonly TransitToDelegate TransitTo;

        public GameRoleStateDomainBaseR(TransitToDelegate transitToDelegate)
        {
            this.TransitTo = transitToDelegate;
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
        }

        public virtual void BindEvents() { }

        public virtual void UnbindEvents() { }

        /** 状态更新 */
        public void Tick(GameRoleEntityR role, float frameTime)
        {
            this._Tick(role, frameTime);
        }

        /** 进入. ps: 直接调用则会跳过了条件判定 */
        public abstract void Enter(GameRoleEntityR role, params object[] args);
        /** 状态更新 */
        protected abstract void _Tick(GameRoleEntityR role, float frameTime);
        /** 退出状态 */
        public virtual void ExitTo(GameRoleEntityR role, GameRoleStateType toState) { }
    }

}


