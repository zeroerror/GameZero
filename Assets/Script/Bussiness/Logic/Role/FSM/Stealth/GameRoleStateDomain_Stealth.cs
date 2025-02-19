using GamePlay.Core;
using GamePlay.Infrastructure;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleStateDomain_Stealth : GameRoleStateDomainBase
    {
        public GameRoleStateDomain_Stealth() : base() { }

        public override bool CheckEnter(GameRoleEntity role, params object[] args)
        {
            return true;
        }

        public override void Enter(GameRoleEntity role, params object[] args)
        {
            var fsmCom = role.fsmCom;
            var duration = (float)args[0];
            duration = GameMathF.Max(duration, fsmCom.stealthState.duration);
            fsmCom.EnterStealth(duration);
            // 提交RC
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_ENTER_STEALTH, new GameRoleRCArgs_StateEnterStealth
            {
                fromStateType = fsmCom.stateType,
                idArgs = role.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameRoleEntity role, float dt)
        {
            var stealthState = role.fsmCom.stealthState;
            stealthState.stateTime += dt;
            var inputCom = role.inputCom;
            if (inputCom.TryGetInputArgs(out var inputArgs))
            {
                stealthState.isMoving = GameRoleMoveUtil.TickMove(role, inputArgs.moveDir, inputArgs.moveDst, dt);
            }
            if (stealthState.isMovingDirty)
            {
                stealthState.isMovingDirty = false;
                // 提交RC
                this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_STATE_STEALTH_MOVE, new GameRoleRCArgs_StateStealthMove
                {
                    idArgs = role.idCom.ToArgs(),
                    isMoving = stealthState.isMoving,
                });
            }
        }

        protected override GameRoleStateType _CheckExit(GameRoleEntity role)
        {
            // 施法时退出
            var inputCom = role.inputCom;
            if (inputCom.TryGetInputArgs(out var inputArgs))
            {
                if (inputArgs.skillId != 0) return GameRoleStateType.Cast;
            }
            // 时间结束退出
            var stealthState = role.fsmCom.stealthState;
            if (stealthState.stateTime >= stealthState.duration)
            {
                if (inputCom.TryGetInputArgs(out inputArgs))
                {
                    if (inputArgs.HasMoveInput()) return GameRoleStateType.Move;
                }
                return GameRoleStateType.Idle;
            }
            return GameRoleStateType.None;
        }
    }
}