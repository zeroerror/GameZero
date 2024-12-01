namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_Explode : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity entity)
        {
            var fsmCom = entity.fsmCom;
            fsmCom.EnterExplode();
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity entity)
        {
            return GameProjectileStateType.None;
        }

        protected override void _Tick(GameProjectileEntity entity, float frameTime)
        {
            throw new System.NotImplementedException();
        }
    }

}