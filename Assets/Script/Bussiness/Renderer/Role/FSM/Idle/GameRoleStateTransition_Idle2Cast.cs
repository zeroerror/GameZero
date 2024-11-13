using GamePlay.Core;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateTransition_Idle2Cast : GameStateTransition
    {
        public override string fromState => "Idle";
        public override string toState => "Cast";

        public override bool Condition()
        {
            return false;
        }

    }
}
