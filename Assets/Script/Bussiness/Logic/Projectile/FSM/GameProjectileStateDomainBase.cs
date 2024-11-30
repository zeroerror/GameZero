namespace GamePlay.Bussiness.Logic
{

    public abstract class GameProjectileStateDomainBase
    {
        protected GameContext _context;

        public GameProjectileStateDomainBase() { }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        /** 尝试进入状态 */
        public bool TryEnter(GameProjectileEntity entity)
        {
            if (!this.CheckEnter(entity)) return false;
            this.Enter(entity);
            return true;
        }

        /** 状态更新 */
        public void Tick(GameProjectileEntity entity, float frameTime)
        {
            this._Tick(entity, frameTime);
            var toState = this._CheckExit(entity);
            if (toState != GameProjectileStateType.None)
            {
                this._context.domainApi.projectileApi.fsmApi.TryEnter(entity, toState);
            }
        }

        /** 判定进入条件 */
        public abstract bool CheckEnter(GameProjectileEntity entity);
        /** 进入. ps: 直接调用则会跳过了条件判定 */
        public abstract void Enter(GameProjectileEntity entity);
        /** 状态更新 */
        protected abstract void _Tick(GameProjectileEntity entity, float frameTime);
        /** 判定退出条件 */
        protected abstract GameProjectileStateType _CheckExit(GameProjectileEntity entity);
        /** 退出状态 */
        public virtual void ExitTo(GameProjectileEntity entity, GameProjectileStateType toState) { }
    }

}