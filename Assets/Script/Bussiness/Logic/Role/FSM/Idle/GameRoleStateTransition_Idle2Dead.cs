using GamePlay.Core;
using System;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateTransition_Idle2Dead : GameStateTransition<GameRoleEntity>
    {
        public GameRoleStateTransition_Idle2Dead(GameRoleStateDomain_Idle fromState, GameRoleStateDomain_Dead toState, Action<GameRoleEntity> onStateChange) : base(fromState, toState, onStateChange) { }

        public override bool TickCondition(float dt, GameRoleEntity role)
        {
            return false;
        }

    }
}
