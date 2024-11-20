using GameVec2 = UnityEngine.Vector2;
using GamePlay.Bussiness.Logic;

namespace GamePlay.Core
{
    public class GameCollisionModel
    {
        private GameColliderBase _colliderA;
        private GameColliderBase _colliderB;

        public GameColliderBase colliderA => _colliderA;

        public GameColliderBase colliderB => _colliderB;

        public GameCollisionType collisionType;

        public GameVec2 mtv;

        public GameCollisionModel(GameColliderBase colliderA, GameColliderBase colliderB, GameCollisionType collisionType, in GameVec2 mtv)
        {
            _colliderA = colliderA;
            _colliderB = colliderB;
            this.collisionType = collisionType;
            this.mtv = mtv;
        }
    }
}
