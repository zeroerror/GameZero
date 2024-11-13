using GamePlay.Core;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateTransition_Any2Dead : GameStateTransition
    {
        public override string fromState => "Any";
        public override string toState => "Dead";

        public override bool Condition()
        {
            return false;
        }

    }
}
