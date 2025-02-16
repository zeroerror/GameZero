using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Render
{
    public class GameDirectorStateDomain_SettlingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_SettlingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        public override void BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_SETTLING, this._OnStateEnter);
        }

        public override void UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_SETTLING, this._OnStateEnter);
        }

        private void _OnStateEnter(object args)
        {
            this.Enter(this._context.director, args);
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
            var rcArgs = (GameDirectorRCArgs_StateEnterSettling)args;
            director.fsmCom.EnterSettling(rcArgs.playerCount, rcArgs.enemyCount, rcArgs.isWin);
            GameLogger.DebugLog($"R 导演 - 进入结算状态 - 玩家: {rcArgs.playerCount} 个, 敌人: {rcArgs.enemyCount} 个, 结果: {(rcArgs.isWin ? "胜利" : "失败")}");
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

        public override void ExitTo(GameDirectorEntityR director, GameDirectorStateType toState)
        {
        }

    }
}