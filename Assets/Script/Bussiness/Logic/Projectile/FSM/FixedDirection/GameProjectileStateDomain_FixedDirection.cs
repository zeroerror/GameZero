namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_FixedDirection : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity entity)
        {
            var fsmCom = entity.fsmCom;
            fsmCom.EnterFixedDirection();
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity entity)
        {
            return GameProjectileStateType.None;
        }

        protected override void _Tick(GameProjectileEntity projectile, float frameTime)
        {
            var transformCom = projectile.transformCom;
            var speed = projectile.attributeCom.GetValue(GameAttributeType.MoveSpeed);
            var direction = transformCom.forward;
            var delta = direction * speed * frameTime;
            transformCom.position += delta;
            projectile.FaceTo(direction);
        }
    }

}