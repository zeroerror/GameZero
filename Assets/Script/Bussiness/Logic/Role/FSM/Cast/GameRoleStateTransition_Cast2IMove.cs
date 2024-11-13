using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
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
