using System.Collections.Generic;
using GamePlay.Core;
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

        public void Tick(float dt)
        {
            this._physicsContext.ForeachAll((physicsCom) =>
            {
                var collider = physicsCom.collider;
                if (collider != null && collider.isEnable)
                {
                    collider.UpdateTRS();
                    collider.Draw(Color.green);
                }
            });

            this._collisionRecoveryDomain.Tick(dt);
        }

        public void CreatePhysics(GameEntityBase entity, GameColliderModelBase colliderModel, bool isTrigger)
        {
            var physicsCom = entity.physicsCom;
            if (physicsCom.collider != null)
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
            var physicsCom = entity.physicsCom;
            var collider = physicsCom.collider;
            if (collider != null)
            {
                collider.isEnable = false;
                _physicsContext.Remove(physicsCom);
            }
        }

        public List<GameEntityBase> GetOverlapEntities(GameColliderModelBase colliderModel, GameTransformArgs transformArgs)
        {
            this._context.SubmitRC(GameDrawRCCollection.RC_DRAW_COLLIDER_MODEL, new GameRCArgs_DrawColliderModel()
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
                var isOverlap = mtv != GameVec2.zero;
                if (isOverlap)
                {
                    if (overlapEntities == null) overlapEntities = new List<GameEntityBase>();
                    overlapEntities.Add(entity);
                    colliderModel.Draw(transformArgs, Color.red);
                    collider.Draw(Color.red);
                    this._context.SubmitRC(GameDrawRCCollection.RC_DRAW_COLLIDER_MODEL, new GameRCArgs_DrawColliderModel()
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
