using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public class GamePhysicsDomain : GamePhysicsDomainApi
    {
        GameContext _context;
        GamePhysicsContext _physicsContext => _context.physicsContext;

        public GamePhysicsDomain()
        {
        }

        public void Dispose()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Tick(float dt)
        {
            _physicsContext.Foreach((physicsCom) =>
            {
                var collider = physicsCom.collider;
                if (collider != null && collider.isEnable)
                {
                    collider.UpdateTRS();
                    collider.Draw(Color.green);
                }
            });
        }

        public void CreatePhysics(GameEntityBase entity, GameColliderModelBase colliderModel)
        {
            var physicsCom = entity.physicsCom;
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
    }
}
