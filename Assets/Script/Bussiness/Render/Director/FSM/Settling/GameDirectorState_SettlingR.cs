namespace GamePlay.Bussiness.Render
{
    public class GameDirectorState_SettlingR : GameDirectorStateBaseR
    {
        public int playerCount;
        public int enemyCount;
        public bool isWin;
        public bool stateFinished;

        public GameDirectorState_SettlingR() { }

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