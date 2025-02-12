namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Dead : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Dead() : base() { }

        public override bool CheckEnter(GameRoleEntity role, params object[] args)
        {
            var stateType = role.fsmCom.stateType;
            if (stateType == GameRoleStateType.Dead) return false;
            return true;
        }

        public override void Enter(GameRoleEntity role, params object[] args)
        {
            role.fsmCom.EnterDead();
            role.physicsCom.collider.isEnable = false;
            role.transformCom.isEnable = false;

            // 去除buff
            this._context.domainApi.buffApi.DetachAllBuffs(role);

            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_DEAD, new GameRoleRCArgs_StateEnterDead
            {
                fromStateType = role.fsmCom.stateType,
                idArgs = role.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameRoleEntity role, float frameTime)
        {
            role.fsmCom.deadState.stateTime += frameTime;
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            var stateTime = role.fsmCom.deadState.stateTime;
            if (stateTime >= 2)
            {
                return GameRoleStateType.Destroyed;
            }
            return GameRoleStateType.None;
        }

        public override void ExitTo(GameRoleEntity entity, GameRoleStateType toState)
        {
            base.ExitTo(entity, toState);
            entity.transformCom.isEnable = true;
        }
    }
}