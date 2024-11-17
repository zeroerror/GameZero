namespace GamePlay.Bussiness.Logic
{
    public class GameRoleFSMCom
    {
        public GameRoleStateModel_Idle idleStateModel { get; private set; }
        public GameRoleStateModel_Move moveStateModel { get; private set; }
        public GameRoleStateModel_Cast castStateModel { get; private set; }
        public GameRoleStateModel_Dead deadStateModel { get; private set; }
        public GameRoleFSMCom()
        {
            idleStateModel = new GameRoleStateModel_Idle();
            moveStateModel = new GameRoleStateModel_Move();
            castStateModel = new GameRoleStateModel_Cast();
            deadStateModel = new GameRoleStateModel_Dead();
        }
    }
}

