using GamePlay.Core;
using Unity.VisualScripting;
using UnityEngine.Rendering;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Cast : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Cast() : base() { }

        public override bool CheckEnter(GameRoleEntity entity, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntity entity, params object[] args)
        {
            // 提交RC
            this._context.rcEventService.Submit(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_CAST, new GameRoleRCArgs_StateEnterCast
            {
                fromState = entity.fsmCom.state,
                idArgs = entity.idCom.ToArgs(),
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