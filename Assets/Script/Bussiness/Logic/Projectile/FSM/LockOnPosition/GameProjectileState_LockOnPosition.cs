using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileState_LockOnPosition : GameProjectileStateBase
    {
        public GameProjectileStateModel_LockOnPosition model;
        public void SetModel(in GameProjectileStateModel_LockOnPosition model) => this.model = model;

        public GameVec2 lockOnPosition;

        public override void Clear()
        {
            base.Clear();
            lockOnPosition = GameVec2.zero;
        }
    }
}