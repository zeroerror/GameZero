using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Render
{
    public class GameDirectorStateDomain_LoadingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_LoadingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        public override void BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_LOADING, this._OnStateEnter);
        }

        public override void UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_LOADING, this._OnStateEnter);
        }

        private void _OnStateEnter(object args)
        {
            this.Enter(this._context.director, args);
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
            var rcArgs = (GameDirectorRCArgs_StateEnterLoading)args;
            director.fsmCom.EnterLoading(rcArgs.fieldId);
            GameLogger.DebugLog("R 导演 - 进入加载状态 - 场地ID: " + rcArgs.fieldId);
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

        public override void ExitTo(GameDirectorEntityR director, GameDirectorStateType toState)
        {
            base.ExitTo(director, toState);
            // 更新回合数
            director.curRound++;
            GameLogger.DebugLog("R 导演 - 进入第 " + director.curRound + " 回合");
        }

    }
}