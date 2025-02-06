namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorState_Settling : GameDirectorStateBase
    {
        public int playerCount;
        public int enemyCount;
        public bool isWin;
        public bool stateFinished;

        public GameDirectorState_Settling() { }

        public override void Clear()
        {
            base.Clear();
            playerCount = 0;
            enemyCount = 0;
            isWin = false;
            stateFinished = false;
        }
    }
}