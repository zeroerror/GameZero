using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_DestroyedR : GameRoleStateDomainBaseR
    {
        private static readonly string GAME_RC_EV_NAME = GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_DESTROYED;
        public GameRoleStateDomain_DestroyedR() : base() { }

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
            var evArgs = (GameRoleRCArgs_StateEnterDestroyed)args;
            ref var idArgs = ref evArgs.idArgs;
            var role = this._roleContext.repo.FindByEntityId(idArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GAME_RC_EV_NAME, args);
                return;
            }
            this.Enter(role);
        }

        public override void Enter(GameRoleEntityR entity, params object[] args)
        {
            GameLogger.Log($"角色状态 - 进入销毁状态");
            entity.SetValid(false);// 标记无效, 等待自动实体回收
        }

        protected override void _Tick(GameRoleEntityR entity, float frameTime)
        {
        }
    }
}