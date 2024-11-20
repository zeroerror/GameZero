using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public interface GameColliderModelBase
    {
        public float angle { get; }
        public GameVec2 offset { get; }
    }
}