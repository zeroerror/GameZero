using System.Collections.Generic;
namespace GamePlay.Bussiness.Renderer
{

    public class GameRoleFSMDomainR
    {
        private GameContextR _context;
        private Dictionary<GameRoleStateType, GameRoleStateDomainBaseR> _stateDomainDict = new Dictionary<GameRoleStateType, GameRoleStateDomainBaseR>();

        public GameRoleFSMDomainR()
        {
            this._stateDomainDict = new Dictionary<GameRoleStateType, GameRoleStateDomainBaseR>(){
                {GameRoleStateType.Idle, new GameRoleStateDomain_IdleR()},
                {GameRoleStateType.Move, new GameRoleStateDomain_MoveR()},
                {GameRoleStateType.Cast, new GameRoleStateDomain_CastR()},
                {GameRoleStateType.Dead, new GameRoleStateDomain_DeadR()},
                {GameRoleStateType.Destroyed, new GameRoleStateDomain_DestroyedR()}
            };
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.Inject(context);
            }
        }


        public void Dispose()
        {
            this.UnbindEvents();
        }


        public void BindEvents()
        {
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.BindEvents();
            }
        }

        public void UnbindEvents()
        {
            foreach (var stateDomain in this._stateDomainDict.Values)
            {
                stateDomain.UnbindEvents();
            }
        }


        public void Tick(GameRoleEntityR role, float dt)
        {
            var fsmCom = role.fsmCom;
            var stateType = fsmCom.stateType;
            if (stateType == GameRoleStateType.None) return;
            if (!this._stateDomainDict.TryGetValue(stateType, out var stateDomain)) return;
            stateDomain.Tick(role, dt);
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