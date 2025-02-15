using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Render
{
    public class GameRoleStateDomain_StealthR : GameRoleStateDomainBaseR
    {
        private static readonly string GAME_RC_EV_NAME = GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_STEALTH;

        public override void BindEvents()
        {
            base.BindEvents();
            this._context.BindRC(GAME_RC_EV_NAME, this._OnEnter);
            this._context.BindRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_STEALTH_MOVE, this._OnStealthMove);
        }

        public override void UnbindEvents()
        {
            base.UnbindEvents();
            this._context.UnbindRC(GAME_RC_EV_NAME, this._OnEnter);
            this._context.UnbindRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_STEALTH_MOVE, this._OnStealthMove);
        }

        private void _OnStealthMove(object args)
        {
            var rcArgs = (GameRoleRCArgs_StateStealthMove)args;
            ref var idArgs = ref rcArgs.idArgs;
            var role = this._roleContext.repo.FindByEntityId(idArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_STEALTH_MOVE, args);
                return;
            }
            var isMoving = rcArgs.isMoving;
            if (isMoving)
            {
                this._context.domainApi.roleApi.PlayAnim(role, "move");
            }
            else
            {
                this._context.domainApi.roleApi.PlayAnim(role, "idle");
            }
        }

        private void _OnEnter(object args)
        {
            var rcArgs = (GameRoleRCArgs_StateEnterStealth)args;
            ref var idArgs = ref rcArgs.idArgs;
            var role = this._roleContext.repo.FindByEntityId(idArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GAME_RC_EV_NAME, args);
                return;
            }
            this._context.domainApi.roleApi.fsmApi.Enter(role, GameRoleStateType.Stealth, rcArgs);
        }

        public override void Enter(GameRoleEntityR role, params object[] args)
        {
            role.fsmCom.EnterStealth();
            // 播放隐身特效
            this._context.domainApi.shaderEffectApi.PlayShaderEffect((int)GameShaderEffectType.Stealth, role);
        }

        protected override void _Tick(GameRoleEntityR role, float frameTime)
        {
        }

        public override void ExitTo(GameRoleEntityR role, GameRoleStateType toState)
        {
            base.ExitTo(role, toState);
            // 停止隐身特效
            this._context.domainApi.shaderEffectApi.StopShaderEffects(role);
        }
    }
}