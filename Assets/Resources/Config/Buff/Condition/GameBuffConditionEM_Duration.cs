using GamePlay.Bussiness.Logic;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameBuffConditionEM_Duration
    {
        public bool isEnable;
        public float duration;

        public GameBuffConditionModel_Duration ToModel()
        {
            return new GameBuffConditionModel_Duration(this.duration);
        }
    }
}