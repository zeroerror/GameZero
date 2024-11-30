using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{
    public class GameBoxColliderModel : GameColliderModelBase
    {
        public static GameBoxColliderModel DEFAULT = new GameBoxColliderModel(new GameVec2(0, 0), 0, 1, 1);

        public GameColliderType colliderType => GameColliderType.Box;
        public GameVec2 offset { get; set; }
        public float angle { get; set; }
        public float width;
        public float height;

        public GameBoxColliderModel(in GameVec2 offset, float angle, float width, float height)
        {
            this.offset = offset;
            this.angle = angle;
            this.width = width;
            this.height = height;
        }

        public override string ToString()
        {
            return $"GameBoxColliderModel: offset={this.offset}, angle={this.angle}, width={this.width}, height={this.height}";
        }

    }
}