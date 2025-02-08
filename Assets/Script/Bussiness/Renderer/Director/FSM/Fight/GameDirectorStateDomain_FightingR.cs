using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectorStateDomain_FightingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_FightingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        public override void BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING, this._OnStateEnter);
        }

        public override void UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnter);
        }

        private void _OnStateEnter(object args)
        {
            this.Enter(this._context.director, args);
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
            director.fsmCom.EnterFighting();
            GameLogger.DebugLog("R 导演 - 进入战斗状态");
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

    }
}