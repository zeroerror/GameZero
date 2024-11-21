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

        /** 尝试进入状态 */
        public bool TryEnter(GameRoleEntityR entity, params object[] args)
        {
            if (!this.CheckEnter(entity, args)) return false;
            this.Enter(entity, args);
            return true;
        }

        /** 状态更新 */
        public void Tick(GameRoleEntityR entity, float frameTime)
        {
            this._Tick(entity, frameTime);
            var toState = this._CheckExit(entity);
            // if (toState != GameRoleStateType.None) this._fsmDomain.Enter(entity, toState);
        }

        /** 判定进入条件 */
        public abstract bool CheckEnter(GameRoleEntityR entity, params object[] args);
        /** 进入. ps: 直接调用则会跳过了条件判定 */
        public abstract void Enter(GameRoleEntityR entity, params object[] args);
        /** 状态更新 */
        protected abstract void _Tick(GameRoleEntityR entity, float frameTime);
        /** 判定退出条件 */
        protected abstract GameRoleStateType _CheckExit(GameRoleEntityR entity);
        /** 退出状态 */
        public virtual void ExitTo(GameRoleEntityR entity, GameRoleStateType toState) { }
    }

}


