using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Move : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Move() : base() { }

        public override bool CheckEnter(GameRoleEntity role, params object[] args)
        {
            var curStateType = role.fsmCom.stateType;
            if (curStateType == GameRoleStateType.Move) return false;
            return true;
        }

        public override void Enter(GameRoleEntity role, params object[] args)
        {
            role.fsmCom.EnterMove();
            role.physicsCom.collider.isTrigger = true;

            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_MOVE, new GameRoleRCArgs_StateEnterMove
            {
                fromStateType = role.fsmCom.stateType,
                idArgs = role.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameRoleEntity role, float dt)
        {
            var stateModel = role.fsmCom.moveState;
            stateModel.stateTime += dt;
            GameRoleMoveUtil.TickMove(role, dt, out var moveDir);
            stateModel.stateMoveDir = moveDir;
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            var inputCom = role.inputCom;
            var hasNoInput = !inputCom.TryGetInputArgs(out var inputArgs);
            if (hasNoInput)
            {
                return GameRoleStateType.Idle;
            }
            if (inputArgs.skillId != 0)
            {
                return GameRoleStateType.Cast;
            }
            return GameRoleStateType.None;
        }

        public override void ExitTo(GameRoleEntity role, GameRoleStateType toState)
        {
            role.physicsCom.collider.isTrigger = false;
        }

    }
}