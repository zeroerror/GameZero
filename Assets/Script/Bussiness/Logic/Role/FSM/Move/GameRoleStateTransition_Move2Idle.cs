using GamePlay.Core;
using System;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateTransition_Move2Idle : GameStateTransition<GameRoleEntity>
    {
        public GameRoleStateTransition_Move2Idle(GameRoleStateDomain_Move fromState, GameRoleStateDomain_Idle toState, Action<GameRoleEntity> onStateChange) : base(fromState, toState, onStateChange) { }

        public override bool TickCondition(float dt, GameRoleEntity role)
        {
            return false;
        }

    }
}
