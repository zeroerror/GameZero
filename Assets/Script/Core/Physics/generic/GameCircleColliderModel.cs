using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameCircleColliderModel : GameColliderModelBase
    {
        public static GameCircleColliderModel DEFAULT = new GameCircleColliderModel(new GameVec2(0, 0), 0, 1);

        public GameColliderType colliderType => GameColliderType.Circle;
        public GameVec2 getoffset => this.offset;
        public float getangle => this.angle;

        public GameVec2 offset;
        public float angle;

        public float radius;

        public GameCircleColliderModel(in GameVec2 offset, float angle, float radius)
        {
            this.offset = offset;
            this.angle = angle;
            this.radius = radius;
        }

        // 将圆形碰撞器描述为字符串
        public override string ToString()
        {
            return $"GameCircleColliderModel: offset={this.getoffset}, angle={this.getangle}, radius={this.radius}";
        }
    }
}
