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
            // 提交RC
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_FIXED_DIRECTION, new GameProjectileRCArgs_StateEnterFixedDirection
            {
                fromStateType = fsmCom.stateType,
                idArgs = projectile.idCom.ToArgs(),
                direction = direction,
            });
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