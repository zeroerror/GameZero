namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorTimelineComponent
    {
        public int frameRate
        {
            get => this._frameRate;
            set
            {
                this._frameRate = value;
                this.frameTime = 1.0f / value;
            }
        }
        private int _frameRate;
        public float frameTime { get; private set; }

        public float timeScale { get; private set; } = 1.0f;
        public float gameTime { get; private set; } = 0.0f;
        public int gameFrame { get; private set; } = 0;
        float cacheDt = 0.0f;

        public GameDirectorTimelineComponent(int frameRate = 30)
        {
            this.frameRate = frameRate;
        }

        public bool Tick(float dt)
        {
            this.gameTime += dt * this.timeScale;
            this.cacheDt += dt * this.timeScale;
            if (this.cacheDt < this.frameTime) return false;
            this.cacheDt -= this.frameTime;
            this.gameFrame++;
            return true;
        }
    }
}