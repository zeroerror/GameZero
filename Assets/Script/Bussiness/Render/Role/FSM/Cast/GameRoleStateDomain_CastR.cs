using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine.UIElements;

namespace GamePlay.Bussiness.Render
{
    public class GameRoleStateDomain_CastR : GameRoleStateDomainBaseR
    {
        private static readonly string GAME_RC_EV_NAME = GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_CAST;

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
            var rcArgs = (GameRoleRCArgs_StateEnterCast)args;
            ref var idArgs = ref rcArgs.idArgs;
            var role = this._roleContext.repo.FindByEntityId(idArgs.entityId);
            if (role == null)
            {
                this._context.DelayRC(GAME_RC_EV_NAME, args);
                return;
            }
            role.skillCom.TryGet(rcArgs.skillId, out var skill);
            if (skill.skillModel.effectByAttackSpeed)
            {
                var attackSpeed = role.attributeCom.GetValue(GameAttributeType.AttackSpeed);
                var timeScale = attackSpeed == 0 ? 1 : attackSpeed * skill.skillModel.clipLength;
                role.animCom.timeScale = timeScale;
            }
            this._context.domainApi.roleApi.fsmApi.Enter(role, GameRoleStateType.Cast, rcArgs.skillId);
        }

        public override void Enter(GameRoleEntityR role, params object[] args)
        {
            var skillId = (int)args[0];
            role.skillCom.TryGet(skillId, out var skill);
            this._context.domainApi.roleApi.PlayAnim(role, skill.skillModel.clipName);
            role.fsmCom.EnterCast(skill);
            if (skill.skillModel.skillType == GameSkillType.MagicAttack) role.attributeBarCom.mpSlider.SetActive(false);
        }

        protected override void _Tick(GameRoleEntityR role, float frameTime)
        {
        }

        public override void ExitTo(GameRoleEntityR role, GameRoleStateType toState)
        {
            base.ExitTo(role, toState);
            role.animCom.timeScale = 1;
            var skill = role.fsmCom.castState.skill;
            if (skill.skillModel.skillType == GameSkillType.MagicAttack) role.attributeBarCom.mpSlider.SetActive(true);
        }
    }
}