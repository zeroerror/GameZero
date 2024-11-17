using GamePlay.Core;
using System;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateTransition_Idle2Move : GameStateTransition<GameRoleEntity>
    {
        public GameRoleStateTransition_Idle2Move(GameRoleStateDomain_Idle fromState, GameRoleStateDomain_Move toState, Action<GameRoleEntity> onStateChange) : base(fromState, toState, onStateChange) { }

        public override bool TickCondition(float dt, GameRoleEntity role)
        {
            return false;
        }

    }
}
