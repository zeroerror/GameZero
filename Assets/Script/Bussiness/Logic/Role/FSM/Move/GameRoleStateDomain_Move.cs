using GamePlay.Core;
using UnityEditor.MPE;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Move : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Move() : base() { }

        public override bool CheckEnter(GameRoleEntity role)
        {
            var curStateType = role.fsmCom.stateType;
            if (curStateType == GameRoleStateType.Move) return false;
            return true;
        }

        public override void Enter(GameRoleEntity role)
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

        protected override void _Tick(GameRoleEntity role, float frameTime)
        {
            var stateModel = role.fsmCom.moveState;
            var inputCom = role.inputCom;
            if (inputCom.TryGetInputArgs(out var inputArgs))
            {
                stateModel.inputArgs = inputArgs;
            }
            inputArgs = stateModel.inputArgs;

            // 根据方向移动
            var moveDir = inputArgs.moveDir;
            if (moveDir != GameVec2.zero)
            {
                var moveSpeed = role.attributeCom.GetValue(GameAttributeType.MoveSpeed);
                var moveVec = new GameVec2(moveDir.x, moveDir.y) * moveSpeed * frameTime;
                role.transformCom.position += moveVec;
                role.FaceTo(moveDir);
                return;
            }

            // 根据目的地移动
            var moveDst = inputArgs.moveDst;
            if (moveDst != GameVec2.zero && role.transformCom.position != moveDst)
            {
                var moveSpeed = role.attributeCom.GetValue(GameAttributeType.MoveSpeed);
                var moveVec = moveDst - new GameVec2(role.transformCom.position.x, role.transformCom.position.y);
                var toDstDir = moveVec.normalized;
                var moveDis = moveVec.magnitude;
                if (moveDis < moveSpeed * frameTime)
                {
                    role.transformCom.position = new GameVec2(moveDst.x, moveDst.y);
                    return;
                }
                role.transformCom.position += toDstDir * moveSpeed * frameTime;
                role.FaceTo(toDstDir);
                return;
            }
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