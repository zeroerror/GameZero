using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Cast : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Cast() : base() { }

        public override bool CheckEnter(GameRoleEntity entity)
        {
            var skillId = entity.inputCom.skillId;
            if (skillId == 0) return false;
            return entity.skillCom.TryGet(skillId, out var _);
        }

        public override void Enter(GameRoleEntity entity)
        {
            var skillId = entity.inputCom.skillId;
            entity.skillCom.TryGet(skillId, out var skill);
            var fsmCom = entity.fsmCom;
            fsmCom.EnterCast(skill);
            skill.timelineCom.Play();
            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_CAST, new GameRoleRCArgs_StateEnterCast
            {
                fromState = entity.fsmCom.state,
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