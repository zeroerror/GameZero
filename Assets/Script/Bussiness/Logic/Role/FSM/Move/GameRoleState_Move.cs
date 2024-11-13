using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleState_Move : GameStateBase
    {
        public GameRoleState_Move() : base("Idle") { }

        public override void Enter()
        {
            GameLogger.Log($"GameRoleState_Move Enter ");
        }

        public override void Tick(float dt)
        {
            GameLogger.Log($"GameRoleState_Move Tick ");
        }

        public override void Exit()
        {
            GameLogger.Log($"GameRoleState_Move Exit ");
        }
    }
}