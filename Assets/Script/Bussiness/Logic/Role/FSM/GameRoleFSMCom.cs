namespace GamePlay.Bussiness.Logic
{
    public class GameRoleFSMCom
    {
        /// <summary> 当前状态 </summary>
        public GameRoleStateType stateType { get; private set; }
        /// <summary> 上一个状态 </summary>
        public GameRoleStateType lastStateType { get; private set; }
        /// <summary> 是否是无效状态 </summary>
        public bool isInvalid => stateType == GameRoleStateType.None || stateType == GameRoleStateType.Destroyed;

        public GameRoleState_Any anyState { get; private set; }
        public GameRoleState_Idle idleState { get; private set; }
        public GameRoleState_Move moveState { get; private set; }
        public GameRoleState_Cast castState { get; private set; }
        public GameRoleState_Dead deadState { get; private set; }
        public GameRoleState_Stealth stealthState { get; private set; }
        public GameRoleState_Destroyed destroyedState { get; private set; }

        public GameRoleFSMCom()
        {
            anyState = new GameRoleState_Any();
            idleState = new GameRoleState_Idle();
            moveState = new GameRoleState_Move();
            castState = new GameRoleState_Cast();
            deadState = new GameRoleState_Dead();
            stealthState = new GameRoleState_Stealth();
            destroyedState = new GameRoleState_Destroyed();
        }

        public void EnterIdle()
        {
            this.SwitchToState(GameRoleStateType.Idle);
        }

        public void EnterMove()
        {
            this.SwitchToState(GameRoleStateType.Move);
        }

        public void EnterCast(GameSkillEntity skill)
        {
            this.SwitchToState(GameRoleStateType.Cast);
            this.castState.skill = skill;
        }

        public void EnterDead()
        {
            this.SwitchToState(GameRoleStateType.Dead);
        }

        public void EnterStealth(float duration)
        {
            this.SwitchToState(GameRoleStateType.Stealth);
            this.stealthState.duration = duration;
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
                case GameRoleStateType.Stealth:
                    stealthState.Clear();
                    break;
                case GameRoleStateType.Destroyed:
                    destroyedState.Clear();
                    break;
            }
        }
    }
}

