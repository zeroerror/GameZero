
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleState_Move : GameRoleStateBase
    {
        public GameVec2 stateMoveDir;

        public GameRoleState_Move() { }

        public override void Clear()
        {
            base.Clear();
            this.stateMoveDir = GameVec2.zero;
        }
    }
}