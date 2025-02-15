
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleState_Move : GameRoleStateBase
    {
        public GameVec2 inputMoveDir;
        public GameVec2 inputMoveDst;
        public GameVec2 moveDst;

        public bool isMoving;

        public GameRoleState_Move() { }

        public override void Clear()
        {
            base.Clear();
            this.inputMoveDir = GameVec2.zero;
            this.inputMoveDst = default;
            this.moveDst = default;
            this.isMoving = false;
        }
    }
}