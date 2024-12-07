namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_LockOnEntity : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity projectile)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity projectile)
        {
            var targetEntity = projectile.actionTargeterCom.targetEntity;
            var fsmCom = projectile.fsmCom;
            fsmCom.EnterLockOnEntity();

            var targetPos = targetEntity.transformCom.position;
            var pos = projectile.transformCom.position;
            var offset = targetPos - pos;
            var dir = offset.normalized;
            projectile.FaceTo(dir);

            // 提交RC
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_ENTITY, new GameProjectileRCArgs_StateEnterLockOnEntity
            {
                fromStateType = fsmCom.stateType,
                idArgs = projectile.idCom.ToArgs(),
                targetIdArgs = targetEntity.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameProjectileEntity projectile, float dt)
        {
            var actionTargeterCom = projectile.actionTargeterCom;
            var fsmCom = projectile.fsmCom;
            GameEntityBase lockOnEntity = null;
            var stateModel = fsmCom.lockOnEntityState.model;
            switch (stateModel.targeterType)
            {
                case GameProjectileTargeterType.RoleActor:
                    lockOnEntity = projectile.idCom.parent;
                    break;
                case GameProjectileTargeterType.Target:
                    lockOnEntity = actionTargeterCom.targetEntity;
                    break;
            }
            fsmCom.lockOnEntityState.lockOnEntity = lockOnEntity;

            if (lockOnEntity == null) return;

            var speed = projectile.fsmCom.lockOnEntityState.model.speed;
            var targetPos = lockOnEntity.transformCom.position;
            var pos = projectile.transformCom.position;
            var offset = targetPos - pos;
            var dir = offset.normalized;
            var frameOffset = dir * speed * dt;
            var framePos = pos + frameOffset;
            projectile.transformCom.position = framePos;
            projectile.FaceTo(dir);
        }
    }

}