using GamePlay.Bussiness.Logic;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameBuffConditionEM_TimeInterval
    {
        public bool isEnable;
        public float timeInterval;

        public GameBuffConditionModel_TimeInterval ToModel()
        {
            if (!this.isEnable) return null;
            return new GameBuffConditionModel_TimeInterval(this.timeInterval);
        }
    }
}