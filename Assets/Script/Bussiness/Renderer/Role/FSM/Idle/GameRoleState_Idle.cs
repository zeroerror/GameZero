using GamePlay.Core;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleState_Idle : GameStateBase
    {
        public GameRoleState_Idle() : base("Idle") { }

        public override void Enter()
        {
            GameLogger.Log($"GameRoleState_Idle Enter ");
        }

        public override void Tick(float dt)
        {
            GameLogger.Log($"GameRoleState_Idle Tick ");
        }

        public override void Exit()
        {
            GameLogger.Log($"GameRoleState_Idle Exit ");
        }
    }
}