using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameTransformArgs
    {
        public GameVec2 position;
        public float angle;
        public GameVec2 scale;
        public GameVec2 forward;

        public GameTransformArgs(in GameVec2 position, in GameVec2 scale, float angle, in GameVec2 forward)
        {
            this.position = position;
            this.scale = scale;
            this.angle = angle;
            this.forward = forward;
        }
    }
}
