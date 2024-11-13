using GamePlay.Core;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateTransition_Idle2Move : GameStateTransition
    {
        public override string fromState => "Idle";
        public override string toState => "Move";

        public override bool Condition()
        {
            return false;
        }

    }
}
