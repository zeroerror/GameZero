using GamePlay.Core;
using Unity.VisualScripting;
using UnityEngine.Rendering;

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
            var stateModel = entity.fsmCom.castStateModel;
            stateModel.Clear();
            stateModel.skill = skill;
            // 提交RC
            this._context.rcEventService.Submit(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_CAST, new GameRoleRCArgs_StateEnterCast
            {
                fromState = entity.fsmCom.state,
                idArgs = entity.idCom.ToArgs(),
                skillId = skillId,
            });
        }

        protected override void _Tick(GameRoleEntity entity, float frameTime)
        {
            var stateModel = entity.fsmCom.castStateModel;
            stateModel.stateTime += frameTime;
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity entity)
        {
            // test exit
            var stateModel = entity.fsmCom.castStateModel;
            var isCastOver = stateModel.stateTime > 0.3;
            if (isCastOver) return GameRoleStateType.Idle;
            return GameRoleStateType.None;
        }
    }
}