using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Idle : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Idle() : base() { }

        public override bool CheckEnter(GameRoleEntity entity)
        {
            return true;
        }

        public override void Enter(GameRoleEntity entity)
        {
            entity.fsmCom.EnterIdle();            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_IDLE, new GameRoleRCArgs_StateEnterIdle
            {
                fromStateType = entity.fsmCom.stateType,
                idArgs = entity.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameRoleEntity entity, float frameTime)
        {
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity entity)
        {
            var inputCom = entity.inputCom;
            if (inputCom.TryGetInputArgs(out var inputArgs))
            {
                if (inputArgs.skillId != 0) return GameRoleStateType.Cast;
                if (inputArgs.moveDir != GameVec2.zero) return GameRoleStateType.Move;
                if (inputArgs.choosePoint != GameVec2.zero) return GameRoleStateType.Move;
            }
            return GameRoleStateType.None;
        }

    }
}