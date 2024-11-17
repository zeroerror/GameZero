using GamePlay.Core;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_DeadR : GameRoleStateDomainR
    {

        public override string stateName => "Dead";

        GameRoleContextR _roleContext => this._context.roleContext;

        public GameRoleStateDomain_DeadR(GameContextR context) : base(context)
        {
        }

        public override void Enter(GameRoleEntityR role)
        {
            GameLogger.Log($"GameRoleStateDomain_DeadR Enter ");
            var factory = this._roleContext.factory;
            var animCom = role.animCom;
            if (animCom.hasClip("dead"))
            {
                animCom.Play("dead");
            }
            else
            {
                var clip = factory.LoadAnimationClip(role.idCom.typeId, "dead");
                animCom.Play(clip);
            }
        }

        public override void Tick(float dt, GameRoleEntityR role)
        {
        }

        public override void Exit(GameRoleStateDomainR nextState, GameRoleEntityR role)
        {
            GameLogger.Log($"GameRoleStateDomain_DeadR Exit ");
        }
    }
}