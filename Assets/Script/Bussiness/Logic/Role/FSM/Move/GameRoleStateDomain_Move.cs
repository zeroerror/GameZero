using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Move : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Move() : base() { }

        public override bool CheckEnter(GameRoleEntity entity, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntity entity, params object[] args)
        {
            GameLogger.Log($"Move enter");
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity entity)
        {
            return GameRoleStateType.None;
        }

        protected override void _Tick(GameRoleEntity entity, float frameTime)
        {
        }
    }
}