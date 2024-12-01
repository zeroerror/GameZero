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
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity entity)
        {
            return GameProjectileStateType.None;
        }

        protected override void _Tick(GameProjectileEntity entity, float frameTime)
        {
            var fsmCom = entity.fsmCom;
            var stateModel = fsmCom.idleStateModel;
            stateModel.stateTime += frameTime;
        }
    }

}