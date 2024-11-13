using GamePlay.Core;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateTransition_Cast2Idle : GameStateTransition
    {
        public override string fromState => "Cast";
        public override string toState => "Idle";

        public override bool Condition()
        {
            return false;
        }

    }
}
