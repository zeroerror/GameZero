using System.Collections.Generic;
namespace GamePlay.Bussiness.Renderer
{

    public class GameRoleFSMDomainR
    {
        private GameContextR _context;
        // private GameRoleAnyStateDomain _anyDomain;
        private Dictionary<GameRoleStateType, GameRoleStateDomainBaseR> _stateDomainDict = new Dictionary<GameRoleStateType, GameRoleStateDomainBaseR>();

        public GameRoleFSMDomainR()
        {
            // this._anyDomain = new GameRoleAnyStateDomain(this);
            // this._stateDomainDict.Add(GameRoleStateType.None, new GameRoleStateDomain_None());
            this._stateDomainDict.Add(GameRoleStateType.Idle, new GameRoleStateDomain_IdleR());
            this._stateDomainDict.Add(GameRoleStateType.Move, new GameRoleStateDomain_MoveR());
            this._stateDomainDict.Add(GameRoleStateType.Cast, new GameRoleStateDomain_CastR());
            this._stateDomainDict.Add(GameRoleStateType.Dead, new GameRoleStateDomain_DeadR());
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            // this._anyDomain.Inject(context);
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.Inject(context);
            }
        }

        public void Dispose() { }

        public void Tick(GameRoleEntityR role, float dt)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.stateType;
            if (stateType == GameRoleStateType.None) return;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.Tick(role, dt);
            // this._anyDomain.Tick(role, dt);
        }

        public bool CheckEnter(GameRoleEntityR role, GameRoleStateType state, params object[] args)
        {
            if (!this._stateDomainDict.TryGetValue(state, out var stateDomain)) return false;
            return stateDomain.CheckEnter(role, args);
        }

        public bool TryEnter(GameRoleEntityR role, GameRoleStateType state, params object[] args)
        {
            if (!this._stateDomainDict.TryGetValue(state, out var stateDomain)) return false;
            var check = stateDomain.CheckEnter(role, args);
            if (check) this.Enter(role, state, args);
            return check;
        }

        public void Enter(GameRoleEntityR role, GameRoleStateType state, params object[] args)
        {
            if (!this._stateDomainDict.TryGetValue(state, out var stateDomain)) return;
            this._ExitToState(role, state);
            stateDomain.Enter(role, args);
        }

        private void _ExitToState(GameRoleEntityR role, GameRoleStateType toState)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.stateType;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.ExitTo(role, toState);
        }
    }

}