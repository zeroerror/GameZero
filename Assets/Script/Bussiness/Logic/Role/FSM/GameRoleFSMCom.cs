using System.Collections.Generic;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleFSMCom
    {
        public GameRoleStateType state { get; private set; }
        public GameRoleStateType lastStateType { get; private set; }

        public GameRoleStateModel_Idle idleStateModel { get; private set; }
        public GameRoleStateModel_Move moveStateModel { get; private set; }
        public GameRoleStateModel_Cast castStateModel { get; private set; }
        public GameRoleStateModel_Dead deadStateModel { get; private set; }

        public Dictionary<GameRoleStateType, GameRoleStateModelBase> stateModelDict;
        public GameRoleFSMCom()
        {
            idleStateModel = new GameRoleStateModel_Idle();
            moveStateModel = new GameRoleStateModel_Move();
            castStateModel = new GameRoleStateModel_Cast();
            deadStateModel = new GameRoleStateModel_Dead();
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
            this.castStateModel.skill = skill;
        }

        public void EnterDead()
        {
            this.SwitchToState(GameRoleStateType.Dead);
        }

        public void SwitchToState(GameRoleStateType nextState)
        {
            this.lastStateType = this.state;
            this.state = nextState;
            switch (nextState)
            {
                case GameRoleStateType.Idle:
                    idleStateModel.Clear();
                    break;
                case GameRoleStateType.Move:
                    moveStateModel.Clear();
                    break;
                case GameRoleStateType.Cast:
                    castStateModel.Clear();
                    break;
                case GameRoleStateType.Dead:
                    deadStateModel.Clear();
                    break;
            }
        }
    }
}

