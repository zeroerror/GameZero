using GamePlay.Core;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleState_Cast : GameStateBase
    {
        public GameRoleState_Cast() : base("Cast") { }

        public override void Enter()
        {
            GameLogger.Log($"GameRoleState_Cast Enter ");
        }

        public override void Tick(float dt)
        {
            GameLogger.Log($"GameRoleState_Cast Tick ");
        }

        public override void Exit()
        {
            GameLogger.Log($"GameRoleState_Cast Exit ");
        }
    }
}