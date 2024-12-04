namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_Attach : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity projectile)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity projectile)
        {
            var fsmCom = projectile.fsmCom;
            var targeter = projectile.actionTargeterCom.getCurTargeter();
            fsmCom.EnterAttach(targeter);
            // 提交RC
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_ATTACH, new GameProjectileRCArgs_StateEnterAttach
            {
                fromStateType = fsmCom.stateType,
                idArgs = projectile.idCom.ToArgs(),
                pos = targeter.targetPosition,
                targetIdArgs = targeter.targetEntity.idCom.ToArgs()
            });
        }

        protected override void _Tick(GameProjectileEntity projectile, float frameTime)
        {
            var fsmCom = projectile.fsmCom;
            var targeter = projectile.actionTargeterCom.getCurTargeter();
            var targetEntity = targeter.targetEntity;
            var tarPos = targetEntity.transformCom.position;
            projectile.transformCom.position = tarPos;
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity projectile)
        {
            var targeter = projectile.actionTargeterCom.getCurTargeter();
            var targetEntity = targeter.targetEntity;
            var tarAttr = targetEntity.attributeCom;
            var tarHP = tarAttr.GetValue(GameAttributeType.Hp);
            if (tarHP <= 0) return GameProjectileStateType.Destroyed;
            return GameProjectileStateType.None;
        }
    }

}