using System;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GamePhysicsDomain_CollisionRecovery
    {
        GameContext _context;
        GamePhysicsContext _physicsContext => _context.physicsContext;

        public GamePhysicsDomain_CollisionRecovery()
        {
        }

        public void Destroy()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Tick()
        {
            var physicsComs_notTrigger = _physicsContext.physicsComs_notTrigger;
            for (int i = 0; i < physicsComs_notTrigger.Count; i++)
            {
                var physicsCom1 = physicsComs_notTrigger[i];
                var collider1 = physicsCom1.collider;
                if (collider1 == null || !collider1.isEnable) continue;

                for (int j = i + 1; j < physicsComs_notTrigger.Count; j++)
                {
                    var physicsCom2 = physicsComs_notTrigger[j];
                    var collider2 = physicsCom2.collider;
                    if (collider2 == null || !collider2.isEnable) continue;

                    var isRole1 = collider1.binder.idCom.entityType == GameEntityType.Role;
                    var isRole2 = collider2.binder.idCom.entityType == GameEntityType.Role;
                    if (isRole1 && isRole2)
                    {
                        this._RoleAndRoleResolve(physicsCom1, physicsCom2);
                    }
                    else
                    {
                        this._DefaultResolve(collider1, collider2);
                    }
                }
            }
        }

        /// <summary> 默认的碰撞恢复 </summary>
        private void _DefaultResolve(GameColliderBase collider1, GameColliderBase collider2)
        {
            var mtv = GamePhysicsResolvingUtil.GetResolvingMTV(collider1, collider2);
            if (mtv == GameVec2.zero) return;
            var mtv1 = mtv * 0.5f;
            var mtv2 = -mtv1;
            collider1.binder.transformCom.position += mtv1;
            collider2.binder.transformCom.position += mtv2;
        }

        /// <summary>
        /// 角色间的碰撞恢复
        /// </summary>
        private void _RoleAndRoleResolve(GamePhysicsCom physicsCom1, GamePhysicsCom physicsCom2)
        {
            var mtv = GamePhysicsResolvingUtil.GetResolvingMTV(physicsCom1.collider, physicsCom2.collider);
            if (mtv == GameVec2.zero) return;

            var role1 = physicsCom1.collider.binder as GameRoleEntity;
            var role2 = physicsCom2.collider.binder as GameRoleEntity;
            var stateType1 = role1.fsmCom.stateType;
            var stateType2 = role2.fsmCom.stateType;
            float mtvCoef1 = 0.5f, mtvCoef2 = 0.5f;
            var isMoving1 = stateType1 == GameRoleStateType.Move;
            var isMoving2 = stateType2 == GameRoleStateType.Move;

            if (isMoving1 ^ isMoving2)
            {
                // 仅有1个角色不在移动状态, 不进行碰撞恢复
                mtvCoef1 = 0;
                mtvCoef2 = 0;
            }
            else if (!isMoving1 && !isMoving2)
            {
                // 2个角色都不在移动状态, 进行碰撞恢复
                mtvCoef1 = 0.5f;
                mtvCoef2 = 0.5f;
            }
            else
            {
                // 2个角色都在移动状态, 移动方向非同一边 或者 移动方向在同一边但速度差足够大, 不进行碰撞恢复
                var stateMoveDir1 = role1.fsmCom.moveState.moveDir;
                var stateMoveDir2 = role2.fsmCom.moveState.moveDir;
                var isSameSideDir = GameVec2.Dot(stateMoveDir1, stateMoveDir2) > 0;
                var isSpeedDiffEnough = Math.Abs(role1.attributeCom.GetValue(GameAttributeType.MoveSpeed) - role2.attributeCom.GetValue(GameAttributeType.MoveSpeed)) > 0.5f;
                if (!isSameSideDir || (isSameSideDir && isSpeedDiffEnough))
                {
                    mtvCoef1 = 0;
                    mtvCoef2 = 0;
                }
            }

            var mtv1 = mtv * mtvCoef1;
            var mtv2 = -mtv1 * mtvCoef2;
            role1.transformCom.position += mtv1;
            role2.transformCom.position += mtv2;
        }
    }
}
