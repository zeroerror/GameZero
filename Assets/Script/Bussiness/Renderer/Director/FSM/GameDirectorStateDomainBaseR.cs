using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Render
{
    public struct GameDirectorExitStateArgs
    {
        public GameDirectorStateType toState;
        public object args;

        public GameDirectorExitStateArgs(GameDirectorStateType toState, object args = null)
        {
            this.toState = toState;
            this.args = args;
        }
    }

    public abstract class GameDirectorStateDomainBaseR
    {
        protected GameContextR _context;
        protected GameDirectorDomainR _directorDomain;

        public GameDirectorStateDomainBaseR(GameDirectorDomainR directorDomain)
        {
            this._directorDomain = directorDomain;
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
        }

        public void Destroy()
        {
            this.UnbindEvents();
        }

        public virtual void BindEvents() { }
        public virtual void UnbindEvents() { }

        /** 状态更新 */
        public void Tick(GameDirectorEntityR director, float frameTime)
        {
            this._Tick(director, frameTime);
        }

        /** 进入. ps: 直接调用则会跳过了条件判定 */
        public abstract void Enter(GameDirectorEntityR director, object args = null);
        /** 状态更新 */
        protected abstract void _Tick(GameDirectorEntityR director, float frameTime);
        /** 退出状态 */
        public virtual void ExitTo(GameDirectorEntityR director, GameDirectorStateType toState) { }
    }

}


