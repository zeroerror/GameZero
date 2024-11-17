using GamePlay.Core;
using System;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateTransition_Idle2CastR : GameStateTransition<GameRoleEntityR>
    {
        public GameRoleStateTransition_Idle2CastR(GameRoleStateDomain_IdleR fromState, GameRoleStateDomain_CastR toState, Action<GameRoleEntityR> onStateChange) : base(fromState, toState, onStateChange) { }
        public override bool TickCondition(float dt, GameRoleEntityR role) => false;

    }
}
