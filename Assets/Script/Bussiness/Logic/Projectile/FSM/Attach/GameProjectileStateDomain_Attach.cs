namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_Attach : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity entity)
        {
            var fsmCom = entity.fsmCom;
            fsmCom.EnterAttach();
            // 提交RC
            var targeter = entity.actionTargeterCom.getCurTargeter();
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_ATTACH, new GameProjectileRCArgs_StateEnterAttach
            {
                fromStateType = fsmCom.stateType,
                idArgs = entity.idCom.ToArgs(),
                pos = targeter.targetPosition,
                targetIdArgs = targeter.targetEntity.idCom.ToArgs()
            });
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