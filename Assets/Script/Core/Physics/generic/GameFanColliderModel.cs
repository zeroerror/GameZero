using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameFanColliderModel : GameColliderModelBase
    {
        public GameVec2 offset { get; set; }
        public float angle { get; set; }
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
            return $"GameFanColliderModel: offset={this.offset}, angle={this.angle}, radius={this.radius}, fanAngle={this.fanAngle}";
        }
    }
}
