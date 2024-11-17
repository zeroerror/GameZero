using GamePlay.Core;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_MoveR : GameRoleStateDomainR
    {
        public override string stateName => "Move";
        private GameRoleContextR _roleContext => this._context.roleContext;

        public GameRoleStateDomain_MoveR(GameContextR context) : base(context)
        {
        }

        public override void Enter(GameRoleEntityR role)
        {
            GameLogger.Log($"GameRoleStateDomain_MoveR Enter ");
            var factory = this._roleContext.factory;
            var animCom = role.animCom;
            if (animCom.hasClip("Move"))
            {
                animCom.Play("Move");
            }
            else
            {
                var clip = factory.LoadAnimationClip(role.idCom.typeId, "Move");
                animCom.Play(clip);
            }
        }

        public override void Tick(float dt, GameRoleEntityR role)
        {
        }

        public override void Exit(GameRoleStateDomainR nextState, GameRoleEntityR role)
        {
            GameLogger.Log($"GameRoleStateDomain_MoveR Exit ");
        }
    }
}