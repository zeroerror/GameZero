namespace GamePlay.Bussiness.Logic
{

    public abstract class GameRoleStateDomainBase
    {
        protected GameContext _context;
        protected GameRoleContext _roleContext => this._context.roleContext;

        public GameRoleStateDomainBase() { }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        /** 尝试进入状态 */
        public bool TryEnter(GameRoleEntity role, params object[] args)
        {
            if (!this.CheckEnter(role)) return false;
            this.Enter(role);
            return true;
        }

        /** 状态更新 */
        public void Tick(GameRoleEntity role, float dt)
        {
            this._Tick(role, dt);
            var toState = this._CheckExit(role);
            if (toState != GameRoleStateType.None)
            {
                this._context.domainApi.roleApi.fsmApi.TryEnter(role, toState);
            }
        }

        /** 判定进入条件 */
        public abstract bool CheckEnter(GameRoleEntity role, params object[] args);
        /** 进入. ps: 直接调用则会跳过了条件判定 */
        public abstract void Enter(GameRoleEntity role, params object[] args);
        /** 状态更新 */
        protected abstract void _Tick(GameRoleEntity role, float dt);
        /** 判定退出条件 */
        protected abstract GameRoleStateType _CheckExit(GameRoleEntity role);
        /** 退出状态 */
        public virtual void ExitTo(GameRoleEntity role, GameRoleStateType toState) { }
    }

}


