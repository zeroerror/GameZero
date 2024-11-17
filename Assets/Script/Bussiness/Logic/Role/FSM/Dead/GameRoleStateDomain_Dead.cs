using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Dead : GameRoleStateDomainBase
    {
        public override string stateName => "Dead";

        public GameRoleStateDomain_Dead(GameContext context) : base(context) { }

        public override void Enter(GameRoleEntity role)
        {
            GameLogger.Log($"GameRoleStateDomain_Dead Enter ");
        }

        public override void Tick(float dt, GameRoleEntity role)
        {
        }

        public override void Exit(GameRoleStateDomainBase nextState, GameRoleEntity role)
        {
            GameLogger.Log($"GameRoleStateDomain_Dead Exit ");
        }
    }
}