using GamePlay.Bussiness.Logic;
using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectDomainR
    {
        public GameContextR context { get; private set; }

        public GameRoleDomainR roleDomain { get; private set; }

        public GameDirectDomainR(GameContext logicContext)
        {
            this.context = new GameContextR(logicContext);
            this.roleDomain = new GameRoleDomainR(this.context);
        }

        public void Dispose()
        {
            this.roleDomain.Dispose();
        }

        public void Update(float dt)
        {
            var director = this.context.director;
            director.Tick(dt);
            this._PreTick(dt);
            this._Tick(dt);
        }

        public void LateUpdate(float dt)
        {
            this._LateTick(dt);
        }

        protected void _PreTick(float dt)
        {
            this.context.delayRCEventService.Tick();
            this.context.eventService.Tick();
        }

        protected void _Tick(float dt)
        {
            this._TickDomain(dt);
        }

        protected void _LateTick(float dt)
        {
            var cameraEntity = this.context.cameraEntity;
            cameraEntity.Tick(dt);
        }

        protected virtual void _TickDomain(float dt)
        {
            this.roleDomain.Tick(dt);
        }
    }
}