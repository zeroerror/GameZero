using GamePlay.Core;
using System;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateTransition_Any2DeadR : GameStateTransition<GameRoleEntityR>
    {
        public GameRoleStateTransition_Any2DeadR(GameRoleStateDomainR fromState, GameRoleStateDomain_DeadR toState, Action<GameRoleEntityR> onStateChange) : base(fromState, toState, onStateChange) { }

        public override bool TickCondition(float dt, GameRoleEntityR role) => false;

    }
}
