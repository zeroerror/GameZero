using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
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
