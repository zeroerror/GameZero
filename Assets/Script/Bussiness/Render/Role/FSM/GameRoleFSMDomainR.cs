using System.Collections.Generic;
namespace GamePlay.Bussiness.Render
{

    public class GameRoleFSMDomainR : GameRoleFSMDomainApiR
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
                {GameRoleStateType.Stealth, new GameRoleStateDomain_StealthR()},
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

        public void Destroy()
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

        private void _Enter(GameRoleEntityR role, GameRoleStateType toState, params object[] args)
        {
            if (!this._stateDomainDict.TryGetValue(toState, out var toStateDomain)) return;
            this._ExitToState(role, toState);
            toStateDomain.Enter(role, args);
        }

        private void _ExitToState(GameRoleEntityR role, GameRoleStateType toState)
        {
            var fsmCom = role.fsmCom;
            var curStateType = fsmCom.stateType;
            if (!this._stateDomainDict.TryGetValue(curStateType, out var curStateDomain)) return;
            curStateDomain.ExitTo(role, toState);
        }

        public void Enter(GameRoleEntityR role, GameRoleStateType toState, params object[] args)
        {
            this._Enter(role, toState, args);
        }
    }

}