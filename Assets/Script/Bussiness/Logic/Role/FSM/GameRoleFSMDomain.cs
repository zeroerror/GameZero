using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{

    public class GameRoleFSMDomain : GameRoleFSMDomainApi
    {
        private GameContext _context;
        // private GameRoleAnyStateDomain _anyDomain;
        private Dictionary<GameRoleStateType, GameRoleStateDomainBase> _stateDomainDict = new Dictionary<GameRoleStateType, GameRoleStateDomainBase>();

        public GameRoleFSMDomain()
        {
            // this._anyDomain = new GameRoleAnyStateDomain(this);
            // this._stateDomainDict.Add(GameRoleStateType.None, new GameRoleStateDomain_None());
            this._stateDomainDict.Add(GameRoleStateType.Idle, new GameRoleStateDomain_Idle());
            this._stateDomainDict.Add(GameRoleStateType.Move, new GameRoleStateDomain_Move());
            this._stateDomainDict.Add(GameRoleStateType.Cast, new GameRoleStateDomain_Cast());
            this._stateDomainDict.Add(GameRoleStateType.Dead, new GameRoleStateDomain_Dead());
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            // this._anyDomain.Inject(context);
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.Inject(context);
            }
        }

        public void Dispose() { }

        public void Tick(GameRoleEntity role, float dt)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.state;
            if (stateType == GameRoleStateType.None) return;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.Tick(role, dt);
            // this._anyDomain.Tick(role, dt);
        }

        public bool CheckEnter(GameRoleEntity role, GameRoleStateType state, params object[] args)
        {
            if (!this._stateDomainDict.TryGetValue(state, out var stateDomain)) return false;
            return stateDomain.CheckEnter(role, args);
        }

        public bool TryEnter(GameRoleEntity role, GameRoleStateType state, params object[] args)
        {
            if (!this._stateDomainDict.TryGetValue(state, out var stateDomain)) return false;
            var check = stateDomain.CheckEnter(role, args);
            if (check) this.Enter(role, state, args);
            return check;
        }

        public void Enter(GameRoleEntity role, GameRoleStateType state, params object[] args)
        {
            if (!this._stateDomainDict.TryGetValue(state, out var stateDomain)) return;
            this._ExitToState(role, state);
            stateDomain.Enter(role, args);
            switch (state)
            {
                case GameRoleStateType.Idle:
                    role.fsmCom.EnterIdle();
                    break;
                case GameRoleStateType.Move:
                    role.fsmCom.EnterMove();
                    break;
                case GameRoleStateType.Cast:
                    role.fsmCom.EnterCast();
                    break;
                case GameRoleStateType.Dead:
                    role.fsmCom.EnterDead();
                    break;
            }
        }

        private void _ExitToState(GameRoleEntity role, GameRoleStateType toState)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.state;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.ExitTo(role, toState);
        }
    }

}