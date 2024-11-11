using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameTransformComponentAPI
    {
        public GameVec2 position { get; set; }
        public float scale { get; set; }
        public float angle { get; set; }
        public GameVec2 forward { get; set; }
    }
}
