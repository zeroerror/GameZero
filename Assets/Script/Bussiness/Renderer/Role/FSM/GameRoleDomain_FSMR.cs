using GamePlay.Core;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleDomain_FSMR : GameFSM<GameRoleEntityR>
    {
        private GameContextR _context;
        private GameContext _logicContext => this._context.logicContext;

        private GameRoleStateDomain_IdleR _idleStateDomain;
        private GameRoleStateDomain_MoveR _moveStateDomain;
        private GameRoleStateDomain_CastR _castStateDomain;
        private GameRoleStateDomain_DeadR _deadStateDomain;

        public GameRoleDomain_FSMR(GameContextR context) : base()
        {
            this._context = context;
            this._Init();
            this._BindEvents();
        }

        public override void Dispose()
        {
            this._logicContext.rcEventService.Unregist(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_IDLE, this._OnEnterIdle);
            this._logicContext.rcEventService.Unregist(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_MOVE, this._OnEnterMove);
            this._logicContext.rcEventService.Unregist(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_CAST, this._OnEnterCast);
            this._logicContext.rcEventService.Unregist(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_DEAD, this._OnEnterDead);
        }

        private void _Init()
        {
            var idleState = new GameRoleStateDomain_IdleR(this._context);
            this._idleStateDomain = idleState;
            this.AddState(idleState);
            var moveState = new GameRoleStateDomain_MoveR(this._context);
            this._moveStateDomain = moveState;
            this.AddState(moveState);
            var castState = new GameRoleStateDomain_CastR(this._context);
            this._castStateDomain = castState;
            this.AddState(castState);
            var deadState = new GameRoleStateDomain_DeadR(this._context);
            this._deadStateDomain = deadState;
            this.AddState(deadState);

            this.SetTransition(new GameRoleStateTransition_Idle2MoveR(idleState, moveState, null));
            this.SetTransition(new GameRoleStateTransition_Idle2CastR(idleState, castState, null));
            this.SetTransition(new GameRoleStateTransition_Idle2DeadR(idleState, deadState, null));

            this.SetTransition(new GameRoleStateTransition_Move2IdleR(moveState, idleState, null));
            this.SetTransition(new GameRoleStateTransition_Move2CastR(moveState, castState, null));

            this.SetTransition(new GameRoleStateTransition_Cast2IdleR(castState, idleState, null));
            this.SetTransition(new GameRoleStateTransition_Cast2MoveR(castState, moveState, null));

            this.SetAnyTransition(new GameRoleStateTransition_Any2DeadR(null, deadState, null));
        }

        private void _BindEvents()
        {
            this._logicContext.rcEventService.Regist(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_IDLE, this._OnEnterIdle);
            this._logicContext.rcEventService.Regist(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_MOVE, this._OnEnterMove);
            this._logicContext.rcEventService.Regist(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_CAST, this._OnEnterCast);
            this._logicContext.rcEventService.Regist(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_DEAD, this._OnEnterDead);
        }

        private void _TransitToState(GameRoleStateType fromState, GameRoleStateType toState, GameRoleEntityR roleR)
        {
            var trans = this._FindTransition(fromState, toState);
            this.TransitTo(trans, roleR);
        }

        private GameStateTransition<GameRoleEntityR> _FindTransition(GameRoleStateType fromState, GameRoleStateType toState)
        {
            var trans = this._transitions.Find((transition) => transition.toState.stateName == fromState.ToString() && transition.fromState.stateName == toState.ToString());
            return trans;
        }

        private void _OnEnterIdle(object argsBox)
        {
            var args = (GameRoleRCColllection.GameRoleRCArgs_StateEnterIdle)argsBox;
            var idComArgs = args.idComArg;
            var roleR = this._context.roleContext.repo.FindByEntityId(idComArgs.entityId);
            if (roleR == null)
            {
                // 渲染异步未完成, 延迟事件逻辑
                this._context.delayRCEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_IDLE, argsBox);
                return;
            }
            this._TransitToState(args.fromState, GameRoleStateType.Idle, roleR);
        }

        private void _OnEnterMove(object argsBox)
        {
            var args = (GameRoleRCColllection.GameRoleRCArgs_StateEnterMove)argsBox;
            var idComArgs = args.idComArg;
            var roleR = this._context.roleContext.repo.FindByEntityId(idComArgs.entityId);
            if (roleR == null)
            {
                // 渲染异步未完成, 延迟事件逻辑
                this._context.delayRCEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_MOVE, argsBox);
                return;
            }
            this._TransitToState(args.fromState, GameRoleStateType.Move, roleR);
        }

        private void _OnEnterCast(object argsBox)
        {
            var args = (GameRoleRCColllection.GameRoleRCArgs_StateEnterCast)argsBox;
            var idComArgs = args.idComArg;
            var roleR = this._context.roleContext.repo.FindByEntityId(idComArgs.entityId);
            if (roleR == null)
            {
                // 渲染异步未完成, 延迟事件逻辑
                this._context.delayRCEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_CAST, argsBox);
                return;
            }
            this._TransitToState(args.fromState, GameRoleStateType.Cast, roleR);
        }

        private void _OnEnterDead(object argsBox)
        {
            var args = (GameRoleRCColllection.GameRoleRCArgs_StateEnterDead)argsBox;
            var idComArgs = args.idComArg;
            var roleR = this._context.roleContext.repo.FindByEntityId(idComArgs.entityId);
            if (roleR == null)
            {
                // 渲染异步未完成, 延迟事件逻辑
                this._context.delayRCEventService.Submit(GameRoleRCColllection.RC_GAME_ROLE_STATE_ENTER_DEAD, argsBox);
                return;
            }
            this._TransitToState(args.fromState, GameRoleStateType.Dead, roleR);
        }
    }
}

