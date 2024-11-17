using GamePlay.Core;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_CastR : GameRoleStateDomainR
    {
        public override string stateName => "Cast";

        public GameRoleStateDomain_CastR(GameContextR context) : base(context) { }

        public override void Enter(GameRoleEntityR role)
        {
            GameLogger.Log($"GameRoleStateDomain_Cast Enter ");
        }

        public override void Tick(float dt, GameRoleEntityR role)
        {
        }

        public override void Exit(GameRoleStateDomainR nextState, GameRoleEntityR role)
        {
            GameLogger.Log($"GameRoleStateDomain_Cast Exit ");
        }
    }
}