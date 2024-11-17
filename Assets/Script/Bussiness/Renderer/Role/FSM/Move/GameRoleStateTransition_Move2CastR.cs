using GamePlay.Core;
using System;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateTransition_Move2CastR : GameStateTransition<GameRoleEntityR>
    {
        public GameRoleStateTransition_Move2CastR(GameRoleStateDomain_MoveR fromState, GameRoleStateDomain_CastR toState, Action<GameRoleEntityR> onStateChange) : base(fromState, toState, onStateChange) { }
        public override bool TickCondition(float dt, GameRoleEntityR role) => false;

    }
}
