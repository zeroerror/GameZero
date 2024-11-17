using GamePlay.Core;
using System;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateTransition_Cast2MoveR : GameStateTransition<GameRoleEntityR>
    {
        public GameRoleStateTransition_Cast2MoveR(GameRoleStateDomain_CastR fromState, GameRoleStateDomain_MoveR toState, Action<GameRoleEntityR> onStateChange) : base(fromState, toState, onStateChange) { }

        public override bool TickCondition(float dt, GameRoleEntityR role) => false;

    }
}
