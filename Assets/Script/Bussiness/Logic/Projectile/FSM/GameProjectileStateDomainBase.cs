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
        public bool TryEnter(GameProjectileEntity projectile)
        {
            if (!this.CheckEnter(projectile)) return false;
            this.Enter(projectile);
            return true;
        }

        /** 状态更新 */
        public void Tick(GameProjectileEntity projectile, float dt)
        {
            this._Tick(projectile, dt);
            var toState = this._CheckExit(projectile);
            if (toState != GameProjectileStateType.None)
            {
                this._context.domainApi.projectileApi.fsmApi.TryEnter(projectile, toState);
            }
        }

        /** 判定进入条件 */
        public abstract bool CheckEnter(GameProjectileEntity projectile);
        /** 进入. ps: 直接调用则会跳过了条件判定 */
        public abstract void Enter(GameProjectileEntity projectile);
        /** 状态更新 */
        protected abstract void _Tick(GameProjectileEntity projectile, float frameTime);
        /** 判定退出条件 */
        protected virtual GameProjectileStateType _CheckExit(GameProjectileEntity projectile) { return GameProjectileStateType.None; }
        /** 退出状态 */
        public virtual void ExitTo(GameProjectileEntity projectile, GameProjectileStateType toState) { }
    }

}