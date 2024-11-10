using GameVec2 = UnityEngine.Vector2;
namespace Game.Core
{
    public class GameColliderCircleModel
    {
        public float radius;
        public GameVec2 offset;

        public GameColliderCircleModel(float radius, GameVec2 offset)
        {
            this.radius = radius;
            this.offset = offset;
        }

        // 将圆形碰撞器描述为字符串
        public override string ToString()
        {
            return $"半径: {radius}, 偏移: {offset}";
        }
    }
}
