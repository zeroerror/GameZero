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

        public void Reset()
        {
            this.timeScale = 1.0f;
            this.gameTime = 0.0f;
            this.gameFrame = 0;
            this.cacheDt = 0.0f;
        }

        public int Tick(float dt)
        {
            this.gameTime += dt * this.timeScale;
            this.cacheDt += dt * this.timeScale;
            var frameTime = GameTimeCollection.frameTime;
            if (this.cacheDt < frameTime) return 0;
            var tickCount = (int)(this.cacheDt / frameTime);
            this.gameFrame += tickCount;
            this.cacheDt -= tickCount * frameTime;
            return tickCount;
        }

        public void SetTimeScale(float timeScale)
        {
            this.timeScale = timeScale;
            this.timeScaleDirty = true;
        }

        public void ClearTimeScaleDirty()
        {
            this.timeScaleDirty = false;
        }
        public bool timeScaleDirty { get; private set; } = false;
    }
}