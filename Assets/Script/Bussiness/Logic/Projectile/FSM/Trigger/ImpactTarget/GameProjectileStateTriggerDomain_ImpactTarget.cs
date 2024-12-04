using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerDomain_ImpactTarget
    {
        private GameContext _context;

        public GameProjectileStateTriggerDomain_ImpactTarget()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public bool CheckSatisfied(GameProjectileEntity projectile, GameProjectileStateTriggerEntity_ImpactTarget trigger, float dt)
        {
            var stateType = projectile.fsmCom.stateType;
            var isLockOnEntityState = stateType == GameProjectileStateType.LockOnEntity;
            var isLockOnPositionState = stateType == GameProjectileStateType.LockOnPosition;
            if (!isLockOnEntityState && !isLockOnPositionState)
            {
                GameLogger.LogError("状态触发器[与目标发生碰撞]: 前提条件当前状态必须是锁定实体或锁定位置");
                return false;
            }

            var triggerModel = trigger.model;
            this._context.domainApi.actionApi.TryGetModel(triggerModel.actionId, out var actionModel);
            var selector = actionModel?.selector;
            if (selector == null || !selector.isRangeSelect)
            {
                GameLogger.LogError("状态触发器[与目标发生碰撞]: 需要包含一个范围选取行为");
                return false;
            }

            var selColliderModel = selector.colliderModel;
            if (isLockOnEntityState)
            {
                var lockOnEntity = projectile.fsmCom.lockOnEntityState.lockOnEntity;
                // 1. 检测目标实体的坐标是否在碰撞体内
                if (!triggerModel.checkByTargetCollider) return GamePhysicsResolvingUtil.CheckOverlap(selColliderModel, projectile.transformCom.ToArgs(), lockOnEntity.transformCom.position);
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