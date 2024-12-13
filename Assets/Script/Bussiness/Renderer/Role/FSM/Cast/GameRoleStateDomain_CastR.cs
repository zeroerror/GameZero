using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleStateDomain_CastR : GameRoleStateDomainBaseR
    {
        private static readonly string GAME_RC_EV_NAME = GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_CAST;
        public GameRoleStateDomain_CastR() : base() { }

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
            this.Enter(role, evArgs.skillId);

            role.skillCom.TryGet(evArgs.skillId, out var skill);
            var attackSpeed = role.attributeCom.GetValue(GameAttributeType.AttackSpeed);
            var length = skill.skillModel.animClip.length;
            var timeScale = length / attackSpeed;
            role.animCom.timeScale = timeScale;
        }

        public override void Enter(GameRoleEntityR entity, params object[] args)
        {
            var skillId = (int)args[0];
            entity.skillCom.TryGet(skillId, out var skill);
            var animClip = skill.skillModel.animClip;
            this._context.domainApi.roleApi.PlayAnim(entity, animClip);
        }

        protected override void _Tick(GameRoleEntityR entity, float frameTime)
        {
        }

        public override void ExitTo(GameRoleEntityR role, GameRoleStateType toState)
        {
            role.animCom.timeScale = 1;
            base.ExitTo(role, toState);
        }
    }
}