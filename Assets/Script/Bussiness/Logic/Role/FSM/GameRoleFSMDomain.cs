using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{

    public class GameRoleFSMDomain : GameRoleFSMDomainApi
    {
        private GameContext _context;
        private Dictionary<GameRoleStateType, GameRoleStateDomainBase> _stateDomainDict = new Dictionary<GameRoleStateType, GameRoleStateDomainBase>();

        public GameRoleFSMDomain()
        {
            this._stateDomainDict.Add(GameRoleStateType.Idle, new GameRoleStateDomain_Idle());
            this._stateDomainDict.Add(GameRoleStateType.Move, new GameRoleStateDomain_Move());
            this._stateDomainDict.Add(GameRoleStateType.Cast, new GameRoleStateDomain_Cast());
            this._stateDomainDict.Add(GameRoleStateType.Dead, new GameRoleStateDomain_Dead());
            this._stateDomainDict.Add(GameRoleStateType.Destroyed, new GameRoleStateDomain_Destroyed());
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.Inject(context);
            }
        }

        public void Dispose() { }

        public void Tick(GameRoleEntity role, float dt)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.stateType;
            if (stateType == GameRoleStateType.None) return;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.Tick(role, dt);
        }

        public bool TryEnter(GameRoleEntity role, GameRoleStateType state)
        {
            if (!this._stateDomainDict.TryGetValue(state, out var stateDomain)) return false;
            var check = stateDomain.CheckEnter(role);
            if (check)
            {
                this._ExitToState(role, state);
                stateDomain.Enter(role);
            }
            return check;
        }

        private void _ExitToState(GameRoleEntity role, GameRoleStateType toState)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.stateType;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.ExitTo(role, toState);
        }
    }

}