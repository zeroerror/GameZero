namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_FixedDirection : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity projectile)
        {
            var fsmCom = projectile.fsmCom;
            fsmCom.EnterFixedDirection();
            var direction = projectile.actionTargeterCom.targetDirection;
            projectile.FaceTo(direction);
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity entity)
        {
            return GameProjectileStateType.None;
        }
        protected override void _Tick(GameProjectileEntity projectile, float frameTime)
        {
            var speed = projectile.attributeCom.GetValue(GameAttributeType.MoveSpeed);
            var direction = projectile.actionTargeterCom.targetDirection;
            var delta = direction * speed * frameTime;
            projectile.transformCom.position += delta;
            projectile.FaceTo(direction);
        }
    }

}