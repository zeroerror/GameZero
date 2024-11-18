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
        public bool TryEnter(GameRoleEntity entity, params object[] args)
        {
            if (!this.CheckEnter(entity, args)) return false;
            this.Enter(entity, args);
            return true;
        }

        /** 状态更新 */
        public void Tick(GameRoleEntity entity, float frameTime)
        {
            this._Tick(entity, frameTime);
            var toState = this._CheckExit(entity);
            // if (toState != GameRoleStateType.None) this._fsmDomain.Enter(entity, toState);
        }

        /** 判定进入条件 */
        public abstract bool CheckEnter(GameRoleEntity entity, params object[] args);
        /** 进入. ps: 直接调用则会跳过了条件判定 */
        public abstract void Enter(GameRoleEntity entity, params object[] args);
        /** 状态更新 */
        protected abstract void _Tick(GameRoleEntity entity, float frameTime);
        /** 判定退出条件 */
        protected abstract GameRoleStateType _CheckExit(GameRoleEntity entity);
        /** 退出状态 */
        public virtual void ExitTo(GameRoleEntity entity, GameRoleStateType toState) { }
    }

}


