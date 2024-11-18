namespace GamePlay.Core
{
    public class GameIdService
    {
        private int _autoIncreaseId = 0;
        public int FetchEntityId()
        {
            return ++_autoIncreaseId;
        }
    }
}