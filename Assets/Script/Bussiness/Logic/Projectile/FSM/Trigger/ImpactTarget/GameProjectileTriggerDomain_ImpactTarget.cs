using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerDomain_ImpactTarget
    {
        private GameContext _context;

        public GameProjectileTriggerDomain_ImpactTarget()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public bool CheckSatisfied(GameProjectileEntity projectile, GameProjectileTriggerEntity_ImpactTarget trigger, float dt)
        {
            var stateType = projectile.fsmCom.stateType;
            var isLockOnEntityState = stateType == GameProjectileStateType.LockOnEntity;
            var isLockOnPositionState = stateType == GameProjectileStateType.LockOnPosition;
            if (!isLockOnEntityState && !isLockOnPositionState)
            {
                GameLogger.LogError("状态触发器[与目标发生碰撞]: 前提条件当前状态必须是锁定实体或锁定地点");
                return false;
            }

            var triggerModel = trigger.model;
            var selColliderModel = triggerModel.detectEntitySelector.colliderModel;
            if (isLockOnEntityState)
            {
                var lockOnEntity = projectile.fsmCom.lockOnEntityState.lockOnEntity;
                // 1. 检测目标实体的坐标是否在碰撞体内
                if (!triggerModel.detectByTargetCollider) return GamePhysicsResolvingUtil.CheckOverlap(selColliderModel, projectile.transformCom.ToArgs(), lockOnEntity.transformCom.position);
                // 2. 检测目标实体的碰撞体是否与碰撞体相交
                var lockOnCollider = lockOnEntity.physicsCom.collider;
                var mtv = GamePhysicsResolvingUtil.GetResolvingMTV(lockOnCollider, selColliderModel, projectile.transformCom.ToArgs());
                var isImpact = mtv != GameVec2.zero;
                return isImpact;
            }

            var lockOnPosition = projectile.fsmCom.lockOnPositionState.lockOnPosition;
            return GamePhysicsResolvingUtil.CheckOverlap(selColliderModel, projectile.transformCom.ToArgs(), lockOnPosition);
        }
    }
}