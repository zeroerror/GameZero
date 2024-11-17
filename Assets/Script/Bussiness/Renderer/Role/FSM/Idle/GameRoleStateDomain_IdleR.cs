using GamePlay.Core;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_IdleR : GameRoleStateDomainR
    {
        public override string stateName => "idle";

        public GameRoleStateDomain_IdleR(GameContextR context) : base(context)
        {
        }

        public override void Enter(GameRoleEntityR role)
        {
            GameLogger.Log($"GameRoleStateDomain_IdleR Enter ");
            var factory = this._context.roleContext.factory;
            var animCom = role.animCom;
            if (animCom.hasClip("idle"))
            {
                animCom.Play("idle");
            }
            else
            {
                var clip = factory.LoadAnimationClip(role.idCom.typeId, "idle");
                animCom.Play(clip);
            }
        }

        public override void Tick(float dt, GameRoleEntityR role)
        {
        }

        public override void Exit(GameRoleStateDomainR nextState, GameRoleEntityR role)
        {
            GameLogger.Log($"GameRoleStateDomain_IdleR Exit ");
        }
    }
}