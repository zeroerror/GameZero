namespace GamePlay.Core
{
    public class GameIdService
    {
        private int _autoIncreaseId = 0;
        public int FetchId()
        {
            return ++_autoIncreaseId;
        }
    }
}