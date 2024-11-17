using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Move : GameRoleStateDomainBase
    {
        public override string stateName => "Move";

        public GameRoleStateDomain_Move(GameContext context) : base(context) { }

        public override void Enter(GameRoleEntity role)
        {
            GameLogger.Log($"GameRoleStateDomain_Move Enter ");
        }

        public override void Tick(float dt, GameRoleEntity role)
        {
        }

        public override void Exit(GameRoleStateDomainBase nextState, GameRoleEntity role)
        {
            GameLogger.Log($"GameRoleStateDomain_Move Exit ");
        }
    }
}