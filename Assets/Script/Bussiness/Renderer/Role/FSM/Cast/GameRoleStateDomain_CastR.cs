using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_CastR : GameRoleStateDomainBaseR
    {
        private static readonly string GAME_RC_EV_NAME = GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_CAST;
        public GameRoleStateDomain_CastR(TransitToDelegate transitToDelegate) : base(transitToDelegate) { }

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
            var evArgs = (GameRoleRCArgs_StateEnterCast)args;
            ref var idArgs = ref evArgs.idArgs;
            var role = this._roleContext.repo.FindByEntityId(idArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GAME_RC_EV_NAME, args);
                return;
            }
            role.skillCom.TryGet(evArgs.skillId, out var skill);
            if (skill.skillModel.effectByAttackSpeed)
            {
                var attackSpeed = role.attributeCom.GetValue(GameAttributeType.AttackSpeed);
                var timeScale = attackSpeed == 0 ? 1 : attackSpeed * skill.skillModel.clipLength;
                role.animCom.timeScale = timeScale;
            }
            this.TransitTo(role, GameRoleStateType.Cast, evArgs.skillId);
        }

        public override void Enter(GameRoleEntityR role, params object[] args)
        {
            var skillId = (int)args[0];
            role.skillCom.TryGet(skillId, out var skill);
            this._context.domainApi.roleApi.PlayAnim(role, skill.skillModel.clipName);
            role.fsmCom.EnterCast();
        }

        protected override void _Tick(GameRoleEntityR role, float frameTime)
        {
        }

        public override void ExitTo(GameRoleEntityR role, GameRoleStateType toState)
        {
            role.animCom.timeScale = 1;
            base.ExitTo(role, toState);
        }
    }
}