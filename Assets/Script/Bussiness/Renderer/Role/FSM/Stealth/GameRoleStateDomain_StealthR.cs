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
        }

        public override void UnbindEvents()
        {
            base.UnbindEvents();
            this._context.UnbindRC(GAME_RC_EV_NAME, this._OnEnter);
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
            var isEnemy = role.idCom.campId != this._roleContext.userRole?.idCom.campId;
            if (isEnemy)
            {
                // 对于敌对阵营, 隐藏模型
                role.bodyCom.tmFoot.SetActive(false);
            }
            else
            {
                // 对于友方阵营, 播放隐身特效
                this._context.domainApi.shaderEffectApi.PlayShaderEffect((int)GameShaderEffectType.Stealth, role);
            }
        }

        public override void ExitTo(GameRoleEntityR role, GameRoleStateType toState)
        {
            base.ExitTo(role, toState);
            role.bodyCom.tmFoot.SetActive(true);
            var isEnemy = role.idCom.campId != this._roleContext.userRole?.idCom.campId;
            if (isEnemy)
            {
                // 对于敌对阵营, 显示模型
                role.bodyCom.tmFoot.SetActive(true);
            }
            else
            {
                // 对于友方阵营, 停止隐身特效
                this._context.domainApi.shaderEffectApi.StopShaderEffects(role);
            }
        }
    }
}