using GamePlay.Bussiness.Logic;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameBuffConditionSetEM
    {
        public GameBuffConditionEM_Duration durationEM;
        public GameBuffConditionEM_TimeInterval timeIntervalEM;

        public GameBuffConditionSetModel ToModel()
        {
            return new GameBuffConditionSetModel(
                this.durationEM?.ToModel(),
                this.timeIntervalEM?.ToModel()
            );
        }
    }
}