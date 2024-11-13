using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
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
