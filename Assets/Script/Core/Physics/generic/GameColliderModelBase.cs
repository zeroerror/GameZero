using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public interface GameColliderModelBase
    {
        public GameColliderType colliderType { get; }
        public float getangle { get; }
        public GameVec2 getoffset { get; }
    }
}