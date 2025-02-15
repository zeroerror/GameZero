namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Destroyed : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Destroyed() : base() { }

        public override bool CheckEnter(GameRoleEntity role, params object[] args)
        {
            var curStateType = role.fsmCom.stateType;
            if (curStateType == GameRoleStateType.Destroyed) return false;
            return true;
        }

        public override void Enter(GameRoleEntity role, params object[] args)
        {
            role.fsmCom.EnterDestroyed();

            // 去除buff
            this._context.domainApi.buffApi.DetachAllBuffs(role);

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

        protected override void _Tick(GameRoleEntity role, float dt)
        {
        }
    }
}