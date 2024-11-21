using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Move : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Move() : base() { }

        public override bool CheckEnter(GameRoleEntity role, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntity role, params object[] args)
        {
            // 提交RC
            this._context.rcEventService.Submit(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_MOVE, new GameRoleRCCollection.GameRoleRCArgs_StateEnterMove
            {
                fromState = role.fsmCom.state,
                idArgs = role.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameRoleEntity role, float frameTime)
        {
            var stateModel = role.fsmCom.moveStateModel;
            var inputCom = role.inputCom;
            if (inputCom.TryGetInputArgs(out var inputArgs))
            {
                stateModel.inputArgs = inputArgs;
            }
            inputArgs = stateModel.inputArgs;
            var moveDir = inputArgs.moveDir;
            var moveSpeed = 80;
            var moveVec = new GameVec2(moveDir.x, moveDir.y) * moveSpeed * frameTime;
            role.transformCom.position += moveVec;
            var faceDir = inputArgs.faceDir;
            faceDir = faceDir != GameVec2.zero ? faceDir : moveDir;
            role.transformCom.forward = faceDir;
        }


        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            var inputCom = role.inputCom;
            if (!inputCom.TryGetInputArgs(out var inputArgs))
            {
                return GameRoleStateType.Idle;
            }
            return GameRoleStateType.None;
        }

    }
}