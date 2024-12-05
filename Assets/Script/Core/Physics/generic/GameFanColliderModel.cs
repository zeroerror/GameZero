using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    [System.Serializable]
    public class GameFanColliderModel : GameColliderModelBase
    {
        public static GameFanColliderModel DEFAULT = new GameFanColliderModel(new GameVec2(0, 0), 0, 1, 1);

        public GameColliderType colliderType => GameColliderType.Fan;
        public GameVec2 getoffset => this.offset;
        public float getangle => this.angle;

        public GameVec2 offset;
        public float angle;
        public float radius;
        public float fanAngle;

        public GameFanColliderModel(in GameVec2 offset, float angle, float radius, float fanAngle)
        {
            this.offset = offset;
            this.angle = angle;
            this.radius = radius;
            this.fanAngle = fanAngle;
        }

        public override string ToString()
        {
            return $"GameFanColliderModel: offset={this.getoffset}, angle={this.getangle}, radius={this.radius}, fanAngle={this.fanAngle}";
        }
    }
}
