using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Cast : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Cast() : base() { }

        public override bool CheckEnter(GameRoleEntity entity, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntity entity, params object[] args)
        {
            GameLogger.Log($"Cast enter");
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