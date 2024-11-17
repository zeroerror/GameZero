using System;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleDomain_FSM : GameFSM<GameRoleEntity>
    {
        public GameContext context { get; private set; }

        private GameRoleStateDomain_Idle _idleStateDomain;
        private GameRoleStateDomain_Move _moveStateDomain;
        private GameRoleStateDomain_Cast _castStateDomain;
        private GameRoleStateDomain_Dead _deadStateDomain;

        public GameRoleDomain_FSM(GameContext context) : base()
        {
            this.context = context;
            this._init();
        }

        private void _init()
        {
            var idleStateDomain = new GameRoleStateDomain_Idle(this.context);
            this.AddState(idleStateDomain);
            this._idleStateDomain = idleStateDomain;
            var moveStateDomain = new GameRoleStateDomain_Move(this.context);
            this.AddState(moveStateDomain);
            var castStateDomain = new GameRoleStateDomain_Cast(this.context);
            this.AddState(castStateDomain);
            var deadStateDomain = new GameRoleStateDomain_Dead(this.context);
            this.AddState(deadStateDomain);

            this.SetTransition(new GameRoleStateTransition_Idle2Move(idleStateDomain, moveStateDomain, this._OnStateChange_Idle2Move));
            this.SetTransition(new GameRoleStateTransition_Idle2Cast(idleStateDomain, castStateDomain, this._OnStateChange_Idle2Cast));
            this.SetTransition(new GameRoleStateTransition_Idle2Dead(idleStateDomain, deadStateDomain, this._OnStateChange_Idle2Dead));

            this.SetTransition(new GameRoleStateTransition_Move2Idle(moveStateDomain, idleStateDomain, this._OnStateChange_Move2Idle));
            this.SetTransition(new GameRoleStateTransition_Move2Cast(moveStateDomain, castStateDomain, this._OnStateChange_Move2Cast));

            this.SetTransition(new GameRoleStateTransition_Cast2Idle(castStateDomain, idleStateDomain, this._OnStateChange_Cast2Idle));
            this.SetTransition(new GameRoleStateTransition_Cast2Move(castStateDomain, moveStateDomain, this._OnStateChange_Cast2Move));

            this.SetAnyTransition(new GameRoleStateTransition_Any2Dead(null, deadStateDomain, this._OnStateChange_Any2Dead));
        }

        private GameStateTransition<GameRoleEntity> _FindTransition(string fromState, string toState)
        {
            var trans = this._transitions.Find((transition) => transition.toState.stateName == fromState && transition.fromState.stateName == toState);
            return trans;
        }

        private GameRoleStateDomainBase _FindState(GameRoleStateType state)
        {
            switch (state)
            {
                case GameRoleStateType.Idle:
                    return this._idleStateDomain;
                case GameRoleStateType.Move:
                    return this._moveStateDomain;
                case GameRoleStateType.Cast:
                    return this._castStateDomain;
                case GameRoleStateType.Dead:
                    return this._deadStateDomain;
                default:
                    return null;
            }
        }

        public void EnterState(GameRoleStateType state, GameRoleEntity role)
        {
            if (this.curState == null)
            {
                this.SetDefaultState(this._FindState(state));
                return;
            }

            switch (state)
            {
                case GameRoleStateType.Idle:
                    var transition = this._FindTransition(this.curState.stateName, GameRoleStateType.Idle.ToString());
                    this.TransitTo(transition, role);
                    break;
            }
        }

        private void _OnStateChange_Idle2Move(GameRoleEntity role)
        {
            this.context.rcEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_MOVE, new GameRoleRCColllection.GameRoleRCArgs_StateEnterMove
            {
                fromState = GameRoleStateType.Idle,
                idComArg = role.idCom.ToArgs(),
            });
        }

        private void _OnStateChange_Idle2Cast(GameRoleEntity role)
        {
            this.context.rcEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_CAST, new GameRoleRCColllection.GameRoleRCArgs_StateEnterCast
            {
                fromState = GameRoleStateType.Idle,
                idComArg = role.idCom.ToArgs(),
            });
        }

        private void _OnStateChange_Idle2Dead(GameRoleEntity role)
        {
            this.context.rcEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_DEAD, new GameRoleRCColllection.GameRoleRCArgs_StateEnterDead
            {
                fromState = GameRoleStateType.Idle,
                idComArg = role.idCom.ToArgs(),
            });
        }

        private void _OnStateChange_Move2Idle(GameRoleEntity role)
        {
            this.context.rcEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_IDLE, new GameRoleRCColllection.GameRoleRCArgs_StateEnterIdle
            {
                fromState = GameRoleStateType.Move,
                idComArg = role.idCom.ToArgs(),
            });
        }

        private void _OnStateChange_Move2Cast(GameRoleEntity role)
        {
            this.context.rcEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_CAST, new GameRoleRCColllection.GameRoleRCArgs_StateEnterCast
            {
                fromState = GameRoleStateType.Move,
                idComArg = role.idCom.ToArgs(),
            });
        }

        private void _OnStateChange_Cast2Idle(GameRoleEntity role)
        {
            this.context.rcEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_IDLE, new GameRoleRCColllection.GameRoleRCArgs_StateEnterIdle
            {
                fromState = GameRoleStateType.Cast,
                idComArg = role.idCom.ToArgs(),
            });
        }

        private void _OnStateChange_Cast2Move(GameRoleEntity role)
        {
            this.context.rcEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_MOVE, new GameRoleRCColllection.GameRoleRCArgs_StateEnterMove
            {
                fromState = GameRoleStateType.Cast,
                idComArg = role.idCom.ToArgs(),
            });
        }

        private void _OnStateChange_Any2Dead(GameRoleEntity role)
        {
            this.context.rcEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_DEAD, new GameRoleRCColllection.GameRoleRCArgs_StateEnterDead
            {
                fromState = GameRoleStateType.None,
                idComArg = role.idCom.ToArgs(),
            });
        }


    }
}

