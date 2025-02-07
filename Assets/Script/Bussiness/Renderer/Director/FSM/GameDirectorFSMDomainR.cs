using System.Collections.Generic;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectorFSMDomainR : GameDirectorFSMDomainApiR
    {
        private GameContextR _context;

        private Dictionary<GameDirectorStateType, GameDirectorStateDomainBaseR> _stateDomainDict;

        public GameDirectorFSMDomainR(GameDirectorDomainR directorDomain)
        {
            this._stateDomainDict = new Dictionary<GameDirectorStateType, GameDirectorStateDomainBaseR>
            {
                { GameDirectorStateType.Loading, new GameDirectorStateDomain_LoadingR(directorDomain) },
                { GameDirectorStateType.FightPreparing, new GameDirectorStateDomain_FightPreparingR(directorDomain) },
                { GameDirectorStateType.Fighting, new GameDirectorStateDomain_FightingR(directorDomain) },
                { GameDirectorStateType.Settling, new GameDirectorStateDomain_SettlingR(directorDomain) },
            };
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            foreach (var stateDomainR in this._stateDomainDict.Values)
            {
                stateDomainR.Inject(context);
            }
        }

        public void BindEvents()
        {
            foreach (var stateDomainR in this._stateDomainDict.Values)
            {
                stateDomainR.BindEvents();
            }
        }

        public void UnbindEvents()
        {
            foreach (var stateDomainR in this._stateDomainDict.Values)
            {
                stateDomainR.UnbindEvents();
            }
        }

        public void Destroy()
        {
            foreach (var stateDomainR in this._stateDomainDict.Values)
            {
                stateDomainR.Destroy();
            }
        }

        public void Tick(GameDirectorEntityR director, float dt)
        {
            var fsmCom = director.fsmCom;
            var stateType = fsmCom.stateType;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomainR)) return;
            stateDomainR.Tick(director, dt);
        }

        public void Enter(GameDirectorEntityR director, GameDirectorStateType toState, object args = null)
        {
            if (!this._stateDomainDict.TryGetValue(toState, out var stateDomainR)) return;
            this._ExitToState(director, toState);
            stateDomainR.Enter(director, args);
            return;
        }

        private void _ExitToState(GameDirectorEntityR director, GameDirectorStateType toState)
        {
            var fsmCom = director.fsmCom;
            var stateType = fsmCom.stateType;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomainR)) return;
            stateDomainR.ExitTo(director, toState);
        }
    }
}