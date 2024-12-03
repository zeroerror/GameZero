using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Destroyed : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Destroyed() : base() { }

        public override bool CheckEnter(GameRoleEntity entity)
        {
            return true;
        }

        public override void Enter(GameRoleEntity entity)
        {
            entity.fsmCom.EnterDestroyed();
            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_DEAD, new GameRoleRCArgs_StateEnterDestroyed
            {
                fromStateType = entity.fsmCom.stateType,
                idArgs = entity.idCom.ToArgs(),
            });
            entity.idCom.entityId = -1;
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity entity)
        {
            return GameRoleStateType.None;
        }

        protected override void _Tick(GameRoleEntity entity, float frameTime)
        {
        }
    }
}