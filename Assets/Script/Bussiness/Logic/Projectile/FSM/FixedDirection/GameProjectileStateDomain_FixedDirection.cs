namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_FixedDirection : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Enter(GameProjectileEntity entity)
        {
            throw new System.NotImplementedException();
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity entity)
        {
            throw new System.NotImplementedException();
        }

        protected override void _Tick(GameProjectileEntity entity, float frameTime)
        {
            var transformCom = entity.transformCom;
            var speed = entity.attributeCom.GetValue(GameAttributeType.MoveSpeed);
            var direction = transformCom.forward;
            var delta = direction * speed * frameTime;
            transformCom.position += delta;
        }
    }

}