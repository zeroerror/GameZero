using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Cast : GameRoleStateDomainBase
    {
        public override string stateName => "Cast";

        public GameRoleStateDomain_Cast(GameContext context) : base(context) { }


        public override void Enter(GameRoleEntity role)
        {
            GameLogger.Log($"GameRoleStateDomain_Cast Enter ");
        }

        public override void Tick(float dt, GameRoleEntity role)
        {
        }

        public override void Exit(GameRoleStateDomainBase nextState, GameRoleEntity role)
        {
            GameLogger.Log($"GameRoleStateDomain_Cast Exit ");
        }
    }
}