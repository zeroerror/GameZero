namespace GamePlay.Bussiness.Logic
{
    public class GameSKillDashSpeedModel
    {
        public readonly int frame;
        public readonly float distanceRatio;

        public GameSKillDashSpeedModel(int frame, float speed)
        {
            this.frame = frame;
            this.distanceRatio = speed;
        }
    }
}