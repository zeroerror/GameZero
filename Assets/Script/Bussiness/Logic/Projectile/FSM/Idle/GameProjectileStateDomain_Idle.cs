namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_Idle : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity entity)
        {
            var fsmCom = entity.fsmCom;
            fsmCom.EnterIdle();
            // 提交RC
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_IDLE, new GameProjectileRCArgs_StateEnterIdle
            {
                fromStateType = fsmCom.stateType,
                idArgs = entity.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameProjectileEntity entity, float frameTime)
        {
            var fsmCom = entity.fsmCom;
            var stateModel = fsmCom.idleStateModel;
            stateModel.stateTime += frameTime;
        }
    }

}