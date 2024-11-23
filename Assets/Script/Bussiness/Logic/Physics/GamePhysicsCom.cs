using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GamePhysicsCom
    {
        public GameColliderBase collider { get; private set; }
        public void SetCollider(GameColliderBase collider) => this.collider = collider;

        public GamePhysicsCom()
        {
        }

        public void Reset()
        {
        }
    }
}