using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
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
