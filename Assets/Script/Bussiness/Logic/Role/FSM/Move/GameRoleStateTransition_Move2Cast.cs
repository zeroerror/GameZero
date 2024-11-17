using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateTransition_Move2Cast : GameStateTransition<GameRoleEntity>
    {
        public GameRoleStateTransition_Move2Cast(GameRoleStateDomain_Move fromState, GameRoleStateDomain_Cast toState, System.Action<GameRoleEntity> onStateChange) : base(fromState, toState, onStateChange) { }

        public override bool TickCondition(float dt, GameRoleEntity role)
        {
            return false;
        }

    }
}
