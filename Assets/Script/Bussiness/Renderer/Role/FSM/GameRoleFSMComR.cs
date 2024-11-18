using System.Collections.Generic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleFSMComR
    {
        public GameRoleStateType stateType { get; private set; }
        public GameRoleStateType lastStateType { get; private set; }

        public GameRoleStateModel_IdleR idleStateModel { get; private set; }
        public GameRoleStateModel_MoveR moveStateModel { get; private set; }
        public GameRoleStateModel_CastR castStateModel { get; private set; }
        public GameRoleStateModel_DeadR deadStateModel { get; private set; }

        public Dictionary<GameRoleStateType, GameRoleStateModelBaseR> stateModelDict;
        public GameRoleFSMComR()
        {
            idleStateModel = new GameRoleStateModel_IdleR();
            moveStateModel = new GameRoleStateModel_MoveR();
            castStateModel = new GameRoleStateModel_CastR();
            deadStateModel = new GameRoleStateModel_DeadR();
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

