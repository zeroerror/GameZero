using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateTransition_Idle2Dead : GameStateTransition
    {
        public override string fromState => "Idle";
        public override string toState => "Dead";

        public override bool Condition()
        {
            return false;
        }

    }
}
