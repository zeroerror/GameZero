using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Idle : GameRoleStateDomainBase
    {
        public override string stateName => "Idle";

        public GameRoleStateDomain_Idle(GameContext context) : base(context) { }

        public override void Enter(GameRoleEntity role)
        {
        }

        public override void Tick(float dt, GameRoleEntity role)
        {
        }

        public override void Exit(GameRoleStateDomainBase nextState, GameRoleEntity role)
        {
            GameLogger.Log($"GameRoleStateDomain_Idle Exit ");
        }
    }
}