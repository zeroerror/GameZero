using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Move : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Move() : base() { }

        public override void Enter(GameRoleEntity role, params object[] args)
        {
            if ((args?.Length ?? 0) > 0)
            {
                var dstPos = (GameVec2)args[0];
                role.fsmCom.EnterMove(dstPos);
            }
            else
            {
                role.fsmCom.EnterMove();
            }
            role.colliderPhysicsCom.collider.isTrigger = true;

            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_MOVE, new GameRoleRCArgs_StateEnterMove
            {
                fromStateType = role.fsmCom.stateType,
                idArgs = role.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameRoleEntity role, float dt)
        {
            var state = role.fsmCom.moveState;
            state.stateTime += dt;
            var inputCom = role.inputCom;
            if (inputCom.TryGetInputArgs(out var inputArgs))
            {
                if (inputArgs.moveDir != GameVec2.zero) state.moveDir = inputArgs.moveDir;
                if (inputArgs.moveDst != GameVec2.zero) state.moveDst = inputArgs.moveDst;
            }

            state.isMoving = GameRoleMoveUtil.TickMove(role, state.moveDir, state.moveDst, dt);
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            var inputCom = role.inputCom;
            if (inputCom.skillId != 0) return GameRoleStateType.Cast;
            if (role.fsmCom.moveState.isMoving) return GameRoleStateType.None;
            return GameRoleStateType.Idle;
        }

        public override void ExitTo(GameRoleEntity role, GameRoleStateType toState)
        {
            role.colliderPhysicsCom.collider.isTrigger = false;
        }

    }
}