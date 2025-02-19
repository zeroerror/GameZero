using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Render
{
    public class GameRoleStateDomain_DeadR : GameRoleStateDomainBaseR
    {
        private static readonly string GAME_RC_EV_NAME = GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_DEAD;

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
            var rcArgs = (GameRoleRCArgs_StateEnterDead)args;
            ref var idArgs = ref rcArgs.idArgs;
            var role = this._roleContext.repo.FindByEntityId(idArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GAME_RC_EV_NAME, args);
                return;
            }
            this._context.domainApi.roleApi.fsmApi.Enter(role, GameRoleStateType.Dead, rcArgs);
        }

        public override void Enter(GameRoleEntityR role, params object[] args)
        {
            GameLogger.Log($"角色进入死亡[表现层]：{role.idCom.entityId}");
            this._context.domainApi.roleApi.PlayAnim(role, "dead");
            role.attributeBarCom.SetActive(false);
            role.fsmCom.EnterDead();
        }

        protected override void _Tick(GameRoleEntityR role, float frameTime)
        {
        }
    }
}