using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Dead : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Dead() : base() { }

        public override bool CheckEnter(GameRoleEntity entity, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntity entity, params object[] args)
        {
            GameLogger.Log($"Dead enter");
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