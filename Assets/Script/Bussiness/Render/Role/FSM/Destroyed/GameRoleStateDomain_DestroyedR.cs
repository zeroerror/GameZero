using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Render
{
    public class GameRoleStateDomain_DestroyedR : GameRoleStateDomainBaseR
    {
        private static readonly string GAME_RC_EV_NAME = GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_DESTROYED;

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
            var rcArgs = (GameRoleRCArgs_StateEnterDestroyed)args;
            ref var idArgs = ref rcArgs.idArgs;
            var role = this._roleContext.repo.FindByEntityId(idArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GAME_RC_EV_NAME, args);
                return;
            }
            this._context.domainApi.roleApi.fsmApi.Enter(role, GameRoleStateType.Destroyed, rcArgs);
        }

        public override void Enter(GameRoleEntityR role, params object[] args)
        {
            // 停止所有shader特效
            this._context.domainApi.shaderEffectApi.StopShaderEffects(role);
            GameLogger.Log($"角色状态 - 进入销毁状态");
            role.SetInvalid();// 标记无效, 等待自动实体回收
            role.fsmCom.EnterDestroyed();
        }

        protected override void _Tick(GameRoleEntityR role, float frameTime)
        {
        }
    }
}