using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Cast : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Cast() : base() { }

        public override bool CheckEnter(GameRoleEntity entity)
        {
            var skillId = entity.inputCom.skillId;
            if (skillId == 0) return false;
            if (!entity.skillCom.TryGet(skillId, out var skill)) return false;
            var skillApi = this._context.domainApi.skillApi;
            return skillApi.CheckCastCondition(entity, skill);
        }

        public override void Enter(GameRoleEntity entity)
        {
            var skillId = entity.inputCom.skillId;
            entity.skillCom.TryGet(skillId, out var skill);
            var skillApi = this._context.domainApi.skillApi;
            skillApi.CastSkill(entity, skill);

            var fsmCom = entity.fsmCom;
            fsmCom.EnterCast(skill);

            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_CAST, new GameRoleRCArgs_StateEnterCast
            {
                fromStateType = entity.fsmCom.stateType,
                idArgs = entity.idCom.ToArgs(),
                skillId = skillId,
            });
        }

        protected override void _Tick(GameRoleEntity entity, float frameTime)
        {
            var stateModel = entity.fsmCom.castStateModel;
            var skill = stateModel.skill;
            // 时间轴更新
            var timelineCom = skill.timelineCom;
            timelineCom.Tick(frameTime);
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity entity)
        {
            var stateModel = entity.fsmCom.castStateModel;
            var skill = stateModel.skill;
            var timelineCom = skill.timelineCom;
            if (!timelineCom.isPlaying) return GameRoleStateType.Idle;
            return GameRoleStateType.None;
        }

        public override void ExitTo(GameRoleEntity entity, GameRoleStateType toState)
        {
            base.ExitTo(entity, toState);
        }
    }
}