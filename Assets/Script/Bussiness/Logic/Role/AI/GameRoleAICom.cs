namespace GamePlay.Bussiness.Logic
{
    public class GameRoleAICom
    {
        public GameRoleInputCom inputCom;
        public GameRoleAIStateType aiStateType { get; private set; }
        public GameRoleAIStateType lastAIStateType { get; private set; }

        public GameRoleAIState_Idle idleState;
        public GameRoleAIState_Attack attackState;

        public GameRoleAICom()
        {
            idleState = new GameRoleAIState_Idle();
            attackState = new GameRoleAIState_Attack();
        }

        public void EnterIdle()
        {
            this.SwitchToState(GameRoleAIStateType.Idle);
        }

        public void EnterAttack()
        {
            this.SwitchToState(GameRoleAIStateType.Attack);
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
                default:
                    break;
            }
        }
    }
}