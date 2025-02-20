using System;
using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Render
{
    public class GameDirectorStateDomain_FightPreparingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_FightPreparingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        public override void BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING, this._OnStateEnter);
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
            director.fsmCom.EnterFightPreparing();
            GameLogger.DebugLog("R 导演 - 进入战斗准备状态");
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

        public override void ExitTo(GameDirectorEntityR director, GameDirectorStateType toState)
        {
            base.ExitTo(director, toState);
        }

    }
}