using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateTransition_Move2Cast : GameStateTransition
    {
        public override string fromState => "Move";
        public override string toState => "Cast";

        public override bool Condition()
        {
            return false;
        }

    }
}
