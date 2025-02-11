using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Render
{
    public class GameRoleStateDomain_IdleR : GameRoleStateDomainBaseR
    {
        private static readonly string GAME_RC_EV_NAME = GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_IDLE;
        public GameRoleStateDomain_IdleR(TransitToDelegate transitToDelegate) : base(transitToDelegate) { }

        public override void BindEvents()
        {
            base.BindEvents();
            this._context.BindRC(GAME_RC_EV_NAME, this._OnEnter);
        }

        public override void UnbindEvents()
        {
            base.UnbindEvents();
            this._context.UnbindRC(GAME_RC_EV_NAME, this._OnEnter);
        }

        private void _OnEnter(object args)
        {
            var rcArgs = (GameRoleRCArgs_StateEnterIdle)args;
            ref var idArgs = ref rcArgs.idArgs;
            var role = this._roleContext.repo.FindByEntityId(idArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GAME_RC_EV_NAME, args);
                return;
            }
            this.TransitTo(role, GameRoleStateType.Idle, args);
        }

        public override void Enter(GameRoleEntityR role, params object[] args)
        {
            this._context.domainApi.roleApi.PlayAnim(role, "idle");
            role.fsmCom.EnterIdle();
        }

        protected override void _Tick(GameRoleEntityR entity, float frameTime)
        {
        }
    }
}