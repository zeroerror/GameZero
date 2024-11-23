namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorTimelineComponent
    {
        public float timeScale { get; private set; } = 1.0f;
        public float gameTime { get; private set; } = 0.0f;
        public int gameFrame { get; private set; } = 0;
        float cacheDt = 0.0f;

        public GameDirectorTimelineComponent()
        {
        }

        public bool Tick(float dt)
        {
            this.gameTime += dt * this.timeScale;
            this.cacheDt += dt * this.timeScale;
            var frameTime = GameTimeCollection.frameTime;
            if (this.cacheDt < frameTime) return false;
            this.cacheDt -= frameTime;
            this.gameFrame++;
            return true;
        }
    }
}