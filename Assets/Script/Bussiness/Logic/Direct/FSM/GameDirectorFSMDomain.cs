using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{

    public class GameDirectorFSMDomain : GameDirectorFSMDomainApi
    {
        private GameContext _context;
        private GameDirectorEntity _director => this._context.director;
        private GameDirectorDomain _directorDomain;

        private Dictionary<GameDirectorStateType, GameDirectorStateDomainBase> _stateDomainDict;

        public GameDirectorFSMDomain(GameDirectorDomain directorDomain)
        {
            this._directorDomain = directorDomain;
            this._stateDomainDict = new Dictionary<GameDirectorStateType, GameDirectorStateDomainBase>
            {
                { GameDirectorStateType.Loading, new GameDirectorStateDomain_Loading(directorDomain) },
                { GameDirectorStateType.FightPreparing, new GameDirectorStateDomain_FightPreparing(directorDomain) },
                { GameDirectorStateType.Fighting, new GameDirectorStateDomain_Fighting(directorDomain) },
                { GameDirectorStateType.Settling, new GameDirectorStateDomain_Settling(directorDomain) },
            };
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.Inject(context);
            }
        }

        public void Destroy()
        {
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.Destroy();
            }
        }

        public void Tick(GameDirectorEntity director, float dt)
        {
            var fsmCom = director.fsmCom;
            var stateType = fsmCom.stateType;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.Tick(director, dt);
        }

        public bool TryEnter(GameDirectorEntity director, GameDirectorStateType toState)
        {
            if (!this._stateDomainDict.TryGetValue(toState, out var stateDomain)) return false;
            var check = stateDomain.CheckEnter(director);
            if (check)
            {
                this._ExitToState(director, toState);
                stateDomain.Enter(director);
            }
            return check;
        }

        private void _ExitToState(GameDirectorEntity director, GameDirectorStateType toState)
        {
            var fsmCom = director.fsmCom;
            var stateType = fsmCom.stateType;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.ExitTo(director, toState);
        }
    }
}