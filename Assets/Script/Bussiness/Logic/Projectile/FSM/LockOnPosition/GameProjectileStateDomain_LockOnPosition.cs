using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_LockOnPosition : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity projectile)
        {
            GameVec2 lockOnPosition = GameVec2.zero;
            var fsmCom = projectile.fsmCom;
            var stateModel = fsmCom.lockOnEntityState.model;
            switch (stateModel.targeterType)
            {
                case GameProjectileTargeterType.RoleActor:
                    lockOnPosition = projectile.idCom.parent.transformCom.position;
                    break;
                case GameProjectileTargeterType.Target:
                    lockOnPosition = projectile.actionTargeterCom.targetEntity.transformCom.position;
                    break;
            }
            fsmCom.lockOnPositionState.lockOnPosition = lockOnPosition;

            // 提交RC
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_LOCK_ON_POSITION, new GameProjectileRCArgs_StateEnterLockOnPosition
            {
                fromStateType = fsmCom.stateType,
                idArgs = projectile.idCom.ToArgs(),
                targetPosition = lockOnPosition,
            });
        }

        protected override void _Tick(GameProjectileEntity entity, float frameTime)
        {
        }
    }

}