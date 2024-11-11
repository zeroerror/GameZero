namespace GamePlay.Core
{
    public abstract class GameCameraComBase
    {
        public bool enable { get; private set; } = true;

        public void Tick(float dt)
        {
            if (!this.enable) return;
            this._Tick(dt);
        }
        protected abstract void _Tick(float dt);
        public abstract void Apply();
    }
}