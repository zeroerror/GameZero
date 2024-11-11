using GamePlay.Core;
namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectDomainR
    {
        public GameContextR context { get; private set; } = new GameContextR();

        public GameDirectDomainR() { }

        public void Update(float dt)
        {
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
            director.Tick(dt);
            GameLogger.Log($"导演帧[渲染层] {director.timeScaleCom.gameTime}");
        }

        protected void _LateTick(float dt)
        {
            var cameraEntity = this.context.cameraEntity;
            cameraEntity.Tick(dt);
        }
    }
}