using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Render
{
    public class GameRoleStateDomain_MoveR : GameRoleStateDomainBaseR
    {
        private static readonly string GAME_RC_EV_NAME = GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_MOVE;

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
            var rcArgs = (GameRoleRCArgs_StateEnterMove)args;
            ref var idArgs = ref rcArgs.idArgs;
            var role = this._roleContext.repo.FindByEntityId(idArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GAME_RC_EV_NAME, args);
                return;
            }
            this._context.domainApi.roleApi.fsmApi.Enter(role, GameRoleStateType.Move, rcArgs);
        }

        public override void Enter(GameRoleEntityR role, params object[] args)
        {
            var canMove = role.attributeCom.GetValue(GameAttributeType.MoveSpeed) > 0;
            if (canMove) this._context.domainApi.roleApi.PlayAnim(role, "move");
            role.fsmCom.EnterMove();
        }

        protected override void _Tick(GameRoleEntityR entity, float frameTime)
        {
        }
    }
}