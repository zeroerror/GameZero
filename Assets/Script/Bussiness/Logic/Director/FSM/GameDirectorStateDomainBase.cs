namespace GamePlay.Bussiness.Logic
{
    public struct GameDirectorExitStateArgs
    {
        public GameDirectorStateType toState;
        public object[] args;

        public GameDirectorExitStateArgs(GameDirectorStateType toState, params object[] args)
        {
            this.toState = toState;
            this.args = args;
        }
    }

    public abstract class GameDirectorStateDomainBase
    {
        protected GameContext _context;
        protected GameDirectorDomain _directorDomain;

        public GameDirectorStateDomainBase(GameDirectorDomain directorDomain)
        {
            this._directorDomain = directorDomain;
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            this._BindEvents();
        }

        public void Destroy()
        {
            this._UnbindEvents();
        }

        protected virtual void _BindEvents() { }
        protected virtual void _UnbindEvents() { }

        /** 状态更新 */
        public void Tick(GameDirectorEntity director, float frameTime)
        {
            this._Tick(director, frameTime);
            var exitArgs = this._CheckExit(director);
            if (exitArgs.toState != GameDirectorStateType.None)
            {
                this._context.domainApi.directorApi.directorFSMApi.TryEnter(director, exitArgs.toState, exitArgs.args);
            }
        }

        /** 判定进入条件 */
        public abstract bool CheckEnter(GameDirectorEntity director, params object[] args);
        /** 进入. ps: 直接调用则会跳过了条件判定 */
        public abstract void Enter(GameDirectorEntity director, params object[] args);
        /** 状态更新 */
        protected abstract void _Tick(GameDirectorEntity director, float frameTime);
        /** 判定退出条件 */
        protected abstract GameDirectorExitStateArgs _CheckExit(GameDirectorEntity director);
        /** 退出状态 */
        public virtual void ExitTo(GameDirectorEntity director, GameDirectorStateType toState) { }
    }

}


