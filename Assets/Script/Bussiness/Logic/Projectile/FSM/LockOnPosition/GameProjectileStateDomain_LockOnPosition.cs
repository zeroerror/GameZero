namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_LockOnPosition : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity entity)
        {
            var fsmCom = entity.fsmCom;
            var targetPos = entity.actionTargeterCom.targetPos;
            fsmCom.EnterLockOnPosition(targetPos);

            // 提交RC
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_POSITION, new GameProjectileRCArgs_StateEnterLockOnPosition
            {
                fromStateType = fsmCom.stateType,
                idArgs = entity.idCom.ToArgs(),
                targetPosition = targetPos,
            });
        }

        protected override void _Tick(GameProjectileEntity entity, float frameTime)
        {
            throw new System.NotImplementedException();
        }
    }

}