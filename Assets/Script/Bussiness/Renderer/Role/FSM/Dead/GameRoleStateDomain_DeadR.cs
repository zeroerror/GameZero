using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_DeadR : GameRoleStateDomainBaseR
    {
        public GameRoleStateDomain_DeadR() : base() { }

        public override bool CheckEnter(GameRoleEntityR entity, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntityR entity, params object[] args)
        {
            GameLogger.Log($"DeadR enter");
            var factory = this._roleContext.factory;
            var animCom = entity.animCom;
            if (animCom.hasClip("dead"))
            {
                animCom.Play("dead");
            }
            else
            {
                var clip = factory.LoadAnimationClip(entity.idCom.typeId, "dead");
                animCom.Play(clip);
            }
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntityR entity)
        {
            return GameRoleStateType.None;
        }

        protected override void _Tick(GameRoleEntityR entity, float frameTime)
        {
        }
    }
}