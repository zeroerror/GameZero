using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameTransformArgs
    {
        public GameVec2 position;
        public GameVec2 scale;
        public float angle;
        public GameVec2 forward;
    }
}
