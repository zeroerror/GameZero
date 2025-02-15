using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Idle : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Idle() : base() { }

        public override bool CheckEnter(GameRoleEntity role, params object[] args)
        {
            var curStateType = role.fsmCom.stateType;
            if (curStateType == GameRoleStateType.Idle) return false;
            if (curStateType == GameRoleStateType.Cast && !role.fsmCom.castState.isOver()) return false;
            return true;
        }

        public override void Enter(GameRoleEntity role, params object[] args)
        {
            role.fsmCom.EnterIdle();
            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_IDLE, new GameRoleRCArgs_StateEnterIdle
            {
                fromStateType = role.fsmCom.stateType,
                idArgs = role.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameRoleEntity role, float dt)
        {
            var stateModel = role.fsmCom.idleState;
            stateModel.stateTime += dt;
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            var inputCom = role.inputCom;
            if (inputCom.TryGetInputArgs(out var inputArgs))
            {
                if (inputArgs.HasSkillInput()) return GameRoleStateType.Cast;
                if (inputArgs.HasMoveInput()) return GameRoleStateType.Move;
            }
            return GameRoleStateType.None;
        }

    }
}