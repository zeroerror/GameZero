using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateTransition_Move2Dead : GameStateTransition<GameRoleEntity>
    {
        public GameRoleStateTransition_Move2Dead(GameRoleStateDomain_Move fromState, GameRoleStateDomain_Dead toState, System.Action<GameRoleEntity> onStateChange) : base(fromState, toState, onStateChange) { }

        public override bool TickCondition(float dt, GameRoleEntity role)
        {
            return false;
        }

    }
}
