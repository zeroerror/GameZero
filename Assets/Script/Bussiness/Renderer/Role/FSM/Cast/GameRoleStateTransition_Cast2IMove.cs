using GamePlay.Core;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateTransition_Cast2Move : GameStateTransition
    {
        public override string fromState => "Cast";
        public override string toState => "Move";

        public override bool Condition()
        {
            return false;
        }

    }
}
