using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateTransition_Move2Idle : GameStateTransition
    {
        public override string fromState => "Move";
        public override string toState => "Idle";

        public override bool Condition()
        {
            return false;
        }

    }
}
