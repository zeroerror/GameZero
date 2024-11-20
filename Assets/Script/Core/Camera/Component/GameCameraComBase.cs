namespace GamePlay.Core
{
    public abstract class GameCameraComBase
    {
        public virtual bool enable { get; set; } = true;

        public void Tick(float dt)
        {
            if (!this.enable) return;
            this._Tick(dt);
        }
        protected abstract void _Tick(float dt);
        public void Apply()
        {
            if (!this.enable) return;
            this._Apply();
        }
        protected abstract void _Apply();
    }
}