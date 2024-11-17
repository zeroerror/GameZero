using GamePlay.Core;
using System;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateTransition_Cast2IdleR : GameStateTransition<GameRoleEntityR>
    {
        public GameRoleStateTransition_Cast2IdleR(GameRoleStateDomain_CastR fromState, GameRoleStateDomain_IdleR toState, Action<GameRoleEntityR> onStateChange) : base(fromState, toState, onStateChange) { }

        public override bool TickCondition(float dt, GameRoleEntityR role) => false;

    }
}
