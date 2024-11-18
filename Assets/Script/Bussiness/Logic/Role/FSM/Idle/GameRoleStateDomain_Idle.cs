using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Idle : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Idle() : base() { }

        public override bool CheckEnter(GameRoleEntity entity, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntity entity, params object[] args)
        {
            GameLogger.Log($"Idle enter");
            // 提交RC
            this._context.rcEventService.Submit(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_IDLE, new GameRoleRCCollection.GameRoleRCArgs_StateEnterIdle
            {
                fromState = entity.fsmCom.state,
                idComArgs = entity.idCom.ToArgs(),
            });
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity entity)
        {
            return GameRoleStateType.None;
        }

        protected override void _Tick(GameRoleEntity entity, float frameTime)
        {
            var inputCom = entity.inputCom;
            if (inputCom.HasInput())
            {
                GameLogger.Log($"Has input, change to move =》 {inputCom.moveDir}");
            }
        }
    }
}