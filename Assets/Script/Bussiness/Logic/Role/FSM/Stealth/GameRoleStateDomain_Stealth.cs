using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Stealth : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Stealth() : base() { }

        public override bool CheckEnter(GameRoleEntity role, params object[] args)
        {
            var curStateType = role.fsmCom.stateType;
            if (curStateType == GameRoleStateType.Stealth) return false;
            if (curStateType == GameRoleStateType.Cast && !role.fsmCom.castState.isOver()) return false;
            return true;
        }

        public override void Enter(GameRoleEntity role, params object[] args)
        {
            var duration = (float)args[0];
            role.fsmCom.EnterStealth(duration);
            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_STEALTH, new GameRoleRCArgs_StateEnterStealth
            {
                fromStateType = role.fsmCom.stateType,
                idArgs = role.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameRoleEntity role, float frameTime)
        {
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            // 施法时退出
            var inputCom = role.inputCom;
            if (inputCom.TryGetInputArgs(out var inputArgs))
            {
                if (inputArgs.skillId != 0) return GameRoleStateType.Cast;
            }
            return GameRoleStateType.None;
        }
    }
}