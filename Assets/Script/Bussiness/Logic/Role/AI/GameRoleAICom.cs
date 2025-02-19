using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleAICom
    {
        public GameRoleInputCom inputCom;
        public GameRoleAIStateType aiStateType { get; private set; }
        public GameRoleAIStateType lastAIStateType { get; private set; }

        public GameRoleAIState_Idle idleState { get; private set; }
        public GameRoleAIState_Attack attackState { get; private set; }
        public GameRoleAIState_Follow followState { get; private set; }

        public GameRoleAICom(GameRoleEntity role)
        {
            idleState = new GameRoleAIState_Idle(role);
            attackState = new GameRoleAIState_Attack(role);
            followState = new GameRoleAIState_Follow(role);
        }

        public void EnterIdle()
        {
            this.SwitchToState(GameRoleAIStateType.Idle);
        }

        public void EnterAttack()
        {
            this.SwitchToState(GameRoleAIStateType.Attack);
        }

        public void EnterFollow()
        {
            this.SwitchToState(GameRoleAIStateType.Follow);
        }

        public void SwitchToState(GameRoleAIStateType nextState)
        {
            this.lastAIStateType = this.aiStateType;
            this.aiStateType = nextState;
            switch (nextState)
            {
                case GameRoleAIStateType.Idle:
                    idleState.Clear();
                    break;
                case GameRoleAIStateType.Attack:
                    attackState.Clear();
                    break;
                case GameRoleAIStateType.Follow:
                    followState.Clear();
                    break;
                default:
                    GameLogger.LogError("GameRoleAICom.SwitchToState: 未处理的状态类型：" + nextState);
                    break;
            }
        }
    }
}