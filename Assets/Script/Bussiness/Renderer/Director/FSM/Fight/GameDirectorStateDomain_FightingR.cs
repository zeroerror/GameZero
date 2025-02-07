using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectorStateDomain_FightingR : GameDirectorStateDomainBaseR
    {
        public GameDirectorStateDomain_FightingR(GameDirectorDomainR directorDomain) : base(directorDomain)
        {
        }

        public override void Enter(GameDirectorEntityR director, object args = null)
        {
            GameLogger.DebugLog("导演 - 进入战斗状态");
        }

        protected override void _Tick(GameDirectorEntityR director, float frameTime)
        {
        }

    }
}