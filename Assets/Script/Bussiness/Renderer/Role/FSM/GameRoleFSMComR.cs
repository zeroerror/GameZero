using System.Collections.Generic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleFSMComR
    {
        public GameRoleStateType stateType { get; private set; }
        public GameRoleStateType lastStateType { get; private set; }

        public GameRoleState_IdleR idleState { get; private set; }
        public GameRoleState_MoveR moveState { get; private set; }
        public GameRoleState_CastR castState { get; private set; }
        public GameRoleState_DeadR deadState { get; private set; }

        public Dictionary<GameRoleStateType, GameRoleStateBaseR> stateModelDict;
        public GameRoleFSMComR()
        {
            idleState = new GameRoleState_IdleR();
            moveState = new GameRoleState_MoveR();
            castState = new GameRoleState_CastR();
            deadState = new GameRoleState_DeadR();
        }

        public void EnterIdle()
        {
            this.SwitchToState(GameRoleStateType.Idle, null);
        }

        public void EnterMove()
        {
            this.SwitchToState(GameRoleStateType.Move, null);
        }

        public void EnterCast()
        {
            this.SwitchToState(GameRoleStateType.Cast, null);
        }

        public void EnterDead()
        {
            this.SwitchToState(GameRoleStateType.Dead, null);
        }

        public void SwitchToState(GameRoleStateType nextState, GameRoleEntityR role)
        {
            this.lastStateType = this.stateType;
            this.stateType = GameRoleStateType.Idle;
            switch (nextState)
            {
                case GameRoleStateType.Idle:
                    idleState.Clear();
                    break;
                case GameRoleStateType.Move:
                    moveState.Clear();
                    break;
                case GameRoleStateType.Cast:
                    castState.Clear();
                    break;
                case GameRoleStateType.Dead:
                    deadState.Clear();
                    break;
            }
        }
    }
}

