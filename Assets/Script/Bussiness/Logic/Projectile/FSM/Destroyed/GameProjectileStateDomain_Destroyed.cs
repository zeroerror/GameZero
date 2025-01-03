namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_Destroyed : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity entity)
        {
            var fsmCom = entity.fsmCom;
            fsmCom.EnterDestroyed();
            // 提交RC
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_DESTROYED, new GameProjectileRCArgs_StateEnterDestroyed
            {
                fromStateType = fsmCom.stateType,
                idArgs = entity.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameProjectileEntity entity, float frameTime)
        {
            var fsmCom = entity.fsmCom;
            var stateModel = fsmCom.destroyedState;
            stateModel.stateTime += frameTime;
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity projectile)
        {
            var stateFrame = projectile.fsmCom.destroyedState.stateFrame;
            if (stateFrame == 1)
            {
                projectile.SetInvalid();
            }
            return GameProjectileStateType.None;
        }
    }

}