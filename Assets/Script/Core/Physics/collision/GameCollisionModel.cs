using GameVec2 = UnityEngine.Vector2;
using Game.Bussiness;

namespace Game.Core
{
    public class GameCollisionModel
    {
        private GameCollider _colliderA;
        private GameCollider _colliderB;

        public GameCollider colliderA => _colliderA;

        public GameCollider colliderB => _colliderB;

        public GameCollisionType collisionType;

        public GameVec2 mtv;

        public GameCollisionModel(GameCollider colliderA, GameCollider colliderB, GameCollisionType collisionType, in GameVec2 mtv)
        {
            _colliderA = colliderA;
            _colliderB = colliderB;
            this.collisionType = collisionType;
            this.mtv = mtv;
        }
    }
}
