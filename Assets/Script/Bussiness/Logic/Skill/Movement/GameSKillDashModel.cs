namespace GamePlay.Bussiness.Logic
{
    public class GameSKillDashModel
    {
        public readonly int frame;
        public readonly float distanceRatio;
        public float x;
        public float y;

        public GameSKillDashModel(int frame, float distanceRatio, float x, float y)
        {
            this.frame = frame;
            this.distanceRatio = distanceRatio;
            this.x = x;
            this.y = y;
        }
    }
}