using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameDirectDomain
    {

        public GameContext context { get; private set; } = new GameContext();

        public GameDirector director => this.context.director;

        public GameDirectDomain() { }

        public void Update(float dt)
        {
            var canTick = this.director.Tick(dt);
            if (!canTick) return;
            this._PreTick(dt);
            this._Tick(dt);
        }

        public void LateUpdate(float dt)
        {
            this._LateTick(dt);
        }


        protected void _PreTick(float dt)
        {
            this.context.eventService.Tick();
        }


        protected void _Tick(float dt)
        {
            var director = this.context.director;
            GameLogger.Log($"导演帧 {director.timeScaleCom.gameFrame}");
        }

        protected void _LateTick(float dt)
        {
        }
    }
}