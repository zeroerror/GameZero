using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameColliderFanModel
    {
        public float radius;
        public float fanAngle;
        public float rotateAngle;
        public GameVec2 offset;

        public GameColliderFanModel(float radius, float fanAngle, float rotateAngle, GameVec2 offset)
        {
            this.radius = radius;
            this.fanAngle = fanAngle;
            this.rotateAngle = rotateAngle;
            this.offset = offset;
        }

        public override string ToString()
        {
            return $"半径: {radius}\n扇形角度: {fanAngle}\n旋转角度: {rotateAngle}\n偏移: {offset}";
        }
    }
}
