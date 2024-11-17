using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateTransition_Any2Dead : GameStateTransition<GameRoleEntity>
    {
        public GameRoleStateTransition_Any2Dead(GameRoleStateDomainBase fromState, GameRoleStateDomain_Dead toState, System.Action<GameRoleEntity> onStateChange) : base(fromState, toState, onStateChange) { }

        public override bool TickCondition(float dt, GameRoleEntity role)
        {
            return false;
        }

    }
}
