using System.Collections.Generic;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GamePhysicsDomain : GamePhysicsDomainApi
    {
        GameContext _context;
        GamePhysicsContext _physicsContext => _context.physicsContext;

        private GamePhysicsDomain_CollisionRecovery _collisionRecoveryDomain;

        public GamePhysicsDomain()
        {
            this._collisionRecoveryDomain = new GamePhysicsDomain_CollisionRecovery();
        }

        public void Destroy()
        {
            this._collisionRecoveryDomain.Destroy();
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            this._collisionRecoveryDomain.Inject(context);
        }

        public void Tick()
        {
            this._physicsContext.ForeachAll((physicsCom) =>
            {
                var collider = physicsCom.collider;
                if (collider != null && collider.isEnable)
                {
                    collider.UpdateTRS();
                    this._context.SubmitRC(GameDrawRCCollection.RC_DRAW_COLLIDER_MODEL, new GameRCArgs_DrawColliderModel
                    {
                        colliderModel = collider.colliderModel,
                        transformArgs = collider.binder.transformCom.ToArgs(),
                        color = Color.green,
                        maintainTime = GameTimeCollection.frameTime,
                    });
                }
            });

            this._collisionRecoveryDomain.Tick();
        }

        public void CreatePhysics(GameEntityBase entity, GameColliderModelBase colliderModel, bool isTrigger)
        {
            this.CreatePhysics(entity, entity.physicsCom, colliderModel, isTrigger);
        }

        public void CreatePhysics(GameEntityBase entity, GamePhysicsCom physicsCom, GameColliderModelBase colliderModel, bool isTrigger)
        {
            if (physicsCom?.collider != null)
            {
                return;
            }

            var id = _physicsContext.idService.FetchId();
            GameColliderBase collider = null;
            switch (colliderModel)
            {
                case GameBoxColliderModel boxColliderModel:
                    collider = new GameBoxCollider(entity, boxColliderModel, id);
                    break;
                case GameCircleColliderModel circleColliderModel:
                    collider = new GameCircleCollider(entity, circleColliderModel, id);
                    break;
                case GameFanColliderModel fanColliderModel:
                    collider = new GameFanCollider(entity, fanColliderModel, id);
                    break;
                default:
                    GameLogger.LogError($"CreatePhysics: unknown colliderModel {colliderModel}");
                    break;
            }
            if (collider != null)
            {
                collider.isEnable = true;
                collider.isTrigger = isTrigger;
                physicsCom.SetCollider(collider);
                _physicsContext.Add(physicsCom);
            }
        }

        public void RemovePhysics(GameEntityBase entity)
        {
            this._RemovePhysics(entity.physicsCom);
            if (entity is GameRoleEntity roleEntity)
            {
                this._RemovePhysics(roleEntity.colliderPhysicsCom);
            }
        }
        private void _RemovePhysics(GamePhysicsCom physicsCom)
        {
            var collider = physicsCom.collider;
            if (collider != null)
            {
                collider.isEnable = false;
                _physicsContext.Remove(physicsCom);
            }
        }

        public List<GameEntityBase> GetOverlapEntities(GameColliderModelBase colliderModel, GameTransformArgs transformArgs)
        {
            this._context.SubmitRC(GameDrawRCCollection.RC_DRAW_COLLIDER_MODEL, new GameRCArgs_DrawColliderModel
            {
                colliderModel = colliderModel,
                transformArgs = transformArgs,
                color = Color.red,
                maintainTime = GameTimeCollection.frameTime,
            });
            List<GameEntityBase> overlapEntities = null;
            var roleContext = this._context.roleContext;
            roleContext.repo.ForeachEntities((entity) =>
            {
                var physicsCom = entity.physicsCom;
                var collider = physicsCom.collider;
                if (collider == null || !collider.isEnable) return;
                var mtv = GamePhysicsResolvingUtil.GetResolvingMTV(collider, colliderModel, transformArgs);
                var isOverlap = !mtv.Equals(GameVec2.zero);
                if (isOverlap)
                {
                    if (overlapEntities == null) overlapEntities = new List<GameEntityBase>();
                    overlapEntities.Add(entity);
                    this._context.SubmitRC(GameDrawRCCollection.RC_DRAW_COLLIDER_MODEL, new GameRCArgs_DrawColliderModel
                    {
                        colliderModel = colliderModel,
                        transformArgs = transformArgs,
                        color = Color.red,
                        maintainTime = 0.1f,
                    });
                }
            });
            // ...其余实体类型的碰撞检测
            return overlapEntities;
        }
    }
}
