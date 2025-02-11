using System.Collections.Generic;
namespace GamePlay.Bussiness.Render
{
    public class GameRoleFSMComR
    {
        public GameRoleStateType stateType { get; private set; }
        public GameRoleStateType lastStateType { get; private set; }

        public GameRoleState_IdleR idleState { get; private set; }
        public GameRoleState_MoveR moveState { get; private set; }
        public GameRoleState_CastR castState { get; private set; }
        public GameRoleState_DeadR deadState { get; private set; }
        public GameRoleState_DestroyedR destroyedState { get; private set; }

        public Dictionary<GameRoleStateType, GameRoleStateBaseR> stateModelDict;
        public GameRoleFSMComR()
        {
            idleState = new GameRoleState_IdleR();
            moveState = new GameRoleState_MoveR();
            castState = new GameRoleState_CastR();
            deadState = new GameRoleState_DeadR();
            destroyedState = new GameRoleState_DestroyedR();
        }

        public void EnterIdle()
        {
            this.SwitchToState(GameRoleStateType.Idle);
        }

        public void EnterMove()
        {
            this.SwitchToState(GameRoleStateType.Move);
        }

        public void EnterCast(GameSkillEntityR skill)
        {
            this.SwitchToState(GameRoleStateType.Cast);
            this.castState.skill = skill;
        }

        public void EnterDead()
        {
            this.SwitchToState(GameRoleStateType.Dead);
        }

        public void EnterDestroyed()
        {
            this.SwitchToState(GameRoleStateType.Destroyed);
        }

        public void SwitchToState(GameRoleStateType nextState)
        {
            this.lastStateType = this.stateType;
            this.stateType = nextState;
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
                case GameRoleStateType.Destroyed:
                    destroyedState.Clear();
                    break;
            }
        }
    }
}

