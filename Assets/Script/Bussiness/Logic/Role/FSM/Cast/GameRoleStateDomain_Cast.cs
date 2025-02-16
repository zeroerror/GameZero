using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Cast : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Cast() : base() { }

        public override bool CheckEnter(GameRoleEntity entity, params object[] args)
        {
            var skillId = entity.inputCom.skillId;
            if (skillId == 0) return false;
            if (!entity.skillCom.TryGet(skillId, out var skill)) return false;
            var skillApi = this._context.domainApi.skillApi;
            return skillApi.CheckCastCondition(entity, skill);
        }

        public override void Enter(GameRoleEntity role, params object[] args)
        {
            var skillId = role.inputCom.skillId;
            role.skillCom.TryGet(skillId, out var skill);
            var skillApi = this._context.domainApi.skillApi;
            skillApi.CastSkill(role, skill);
            role.FaceTo(role.inputCom.targeterArgsList[0].targetDirection);

            var fsmCom = role.fsmCom;
            fsmCom.EnterCast(skill);

            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_CAST, new GameRoleRCArgs_StateEnterCast
            {
                fromStateType = role.fsmCom.stateType,
                idArgs = role.idCom.ToArgs(),
                skillId = skillId,
            });
        }

        protected override void _Tick(GameRoleEntity entity, float dt)
        {
            var stateModel = entity.fsmCom.castState;
            stateModel.stateTime += dt;

            // 技能时间轴
            var skill = stateModel.skill;
            float timeScale = 1;
            var timelineCom = skill.timelineCom;
            // 普攻受到攻速的机制
            if (skill.skillModel.effectByAttackSpeed)
            {
                var attackSpeed = entity.attributeCom.GetValue(GameAttributeType.AttackSpeed);
                attackSpeed = GameMathF.Min(attackSpeed, 5);
                var length = timelineCom.length;
                timeScale = attackSpeed == 0 ? 1 : attackSpeed * length;
            }
            timelineCom.Tick(dt * timeScale);
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            var stateModel = role.fsmCom.castState;
            var skill = stateModel.skill;
            var timelineCom = skill.timelineCom;
            if (timelineCom.isPlaying) return GameRoleStateType.None;

            // -> 移动
            var inputCom = role.inputCom;
            if (inputCom.TryGetInputArgs(out var inputArgs))
            {
                if (!inputArgs.moveDir.Equals(GameVec2.zero))
                {
                    return GameRoleStateType.Move;
                }
            }

            // -> 待机
            return GameRoleStateType.Idle;
        }

        public override void ExitTo(GameRoleEntity role, GameRoleStateType toState)
        {
            base.ExitTo(role, toState);
        }
    }
}