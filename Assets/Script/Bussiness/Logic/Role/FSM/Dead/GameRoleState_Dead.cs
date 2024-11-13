using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleState_Dead : GameStateBase
    {
        public GameRoleState_Dead() : base("Dead") { }

        public override void Enter()
        {
            GameLogger.Log($"GameRoleState_Dead Enter ");
        }

        public override void Tick(float dt)
        {
            GameLogger.Log($"GameRoleState_Dead Tick ");
        }

        public override void Exit()
        {
            GameLogger.Log($"GameRoleState_Dead Exit ");
        }
    }
}