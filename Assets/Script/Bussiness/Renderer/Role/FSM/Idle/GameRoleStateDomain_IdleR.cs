using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_IdleR : GameRoleStateDomainBaseR
    {
        public GameRoleStateDomain_IdleR() : base() { }

        public override bool CheckEnter(GameRoleEntityR entity, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntityR entity, params object[] args)
        {
            GameLogger.Log($"IdleR enter");
            var factory = this._roleContext.factory;
            var animCom = entity.animCom;
            if (animCom.hasClip("idle"))
            {
                animCom.Play("idle");
            }
            else
            {
                var clip = factory.LoadAnimationClip(entity.idCom.typeId, "idle");
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