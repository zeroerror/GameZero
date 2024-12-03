using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateModel_LockOnPosition : GameProjectileStateModelBase
    {
        public GameVec2 lockOnPosition { get; private set; }
        public void SetLockOnPosition(GameVec2 lockOnPosition)
        {
            this.lockOnPosition = lockOnPosition;
        }

        public override void Clear()
        {
            base.Clear();
            this.lockOnPosition = GameVec2.zero;
        }
    }
}