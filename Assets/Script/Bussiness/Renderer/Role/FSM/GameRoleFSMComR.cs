namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleFSMComR
    {
        public GameRoleStateModel_IdleR idleStateModel { get; private set; }
        public GameRoleStateModel_MoveR moveStateModel { get; private set; }
        public GameRoleStateModel_CastR castStateModel { get; private set; }
        public GameRoleStateModel_DeadR deadStateModel { get; private set; }
        public GameRoleFSMComR()
        {
            idleStateModel = new GameRoleStateModel_IdleR();
            moveStateModel = new GameRoleStateModel_MoveR();
            castStateModel = new GameRoleStateModel_CastR();
            deadStateModel = new GameRoleStateModel_DeadR();
        }
    }
}

