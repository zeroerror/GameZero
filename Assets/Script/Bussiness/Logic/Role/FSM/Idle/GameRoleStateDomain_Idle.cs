using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Idle : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Idle() : base() { }

        public override bool CheckEnter(GameRoleEntity role)
        {
            var curStateType = role.fsmCom.stateType;
            if (curStateType == GameRoleStateType.Idle) return false;
            return true;
        }

        public override void Enter(GameRoleEntity role)
        {
            role.fsmCom.EnterIdle();            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_IDLE, new GameRoleRCArgs_StateEnterIdle
            {
                fromStateType = role.fsmCom.stateType,
                idArgs = role.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameRoleEntity role, float frameTime)
        {
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            var inputCom = role.inputCom;
            if (inputCom.TryGetInputArgs(out var inputArgs))
            {
                if (inputArgs.skillId != 0) return GameRoleStateType.Cast;
                if (inputArgs.moveDir != GameVec2.zero) return GameRoleStateType.Move;
                // todo 根据技能输入的目标选取器 判断是否进入对应状态
            }
            return GameRoleStateType.None;
        }

    }
}