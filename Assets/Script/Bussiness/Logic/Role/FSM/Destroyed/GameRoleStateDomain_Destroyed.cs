using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Destroyed : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Destroyed() : base() { }

        public override bool CheckEnter(GameRoleEntity role)
        {
            var curStateType = role.fsmCom.stateType;
            if (curStateType == GameRoleStateType.Destroyed) return false;
            return true;
        }

        public override void Enter(GameRoleEntity role)
        {
            role.fsmCom.EnterDestroyed();
            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_DESTROYED, new GameRoleRCArgs_StateEnterDestroyed
            {
                fromStateType = role.fsmCom.stateType,
                idArgs = role.idCom.ToArgs(),
            });
            role.SetInvalid();
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            return GameRoleStateType.None;
        }

        protected override void _Tick(GameRoleEntity role, float frameTime)
        {
        }
    }
}