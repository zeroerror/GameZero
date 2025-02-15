
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleState_Move : GameRoleStateBase
    {
        public GameVec2 moveDir;
        public GameVec2 moveDst;

        public bool isMoving;

        public GameRoleState_Move() { }

        public override void Clear()
        {
            base.Clear();
            this.moveDir = GameVec2.zero;
            this.moveDst = default;
            this.isMoving = false;
        }
    }
}