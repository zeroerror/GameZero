using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public struct GameProjectileStateModel_FixedDirection
    {
        public float speed;

        public GameProjectileStateModel_FixedDirection(float speed)
        {
            this.speed = speed;
        }
    }
}