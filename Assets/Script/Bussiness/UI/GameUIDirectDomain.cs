using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class GameUIDirectDomain
    {
        public GameUIContext context { get; private set; }
        public GameUIDebugDomain debugDomain { get; private set; }
        public GameUIJumpTextDomain jumpTextDomain { get; private set; }
        public GameUILayerDomain layerDomain { get; private set; }

        public GameUIDirectDomain()
        {
            this.context = new GameUIContext();
            this._InitDomain();
        }

        private void _InitDomain()
        {
            this.debugDomain = new GameUIDebugDomain();
            this.jumpTextDomain = new GameUIJumpTextDomain();
            this.layerDomain = new GameUILayerDomain();
        }

        public void Inject(GameObject uiRoot, GameDomainApi logicApi, GameDomainApiR rendererApi)
        {
            this._InitContext(uiRoot, logicApi, rendererApi);
            this._InjectContext();
        }

        private void _InitContext(GameObject uiRoot, GameDomainApi logicApi, GameDomainApiR rendererApi)
        {
            this.context.Inject(uiRoot, logicApi, rendererApi);
            var domainApi = this.context.domainApi;
            domainApi.SetJumpTextDomainApi(this.jumpTextDomain);
            domainApi.SetLayerApi(this.layerDomain);
        }

        private void _InjectContext()
        {
            this.debugDomain.Inject(this.context);
            this.jumpTextDomain.Inject(this.context);
            this.layerDomain.Inject(this.context);
        }

        public void Destroy()
        {
            this.debugDomain.Destroy();
            this.jumpTextDomain.Destroy();
            this.layerDomain.Destroy();
        }

        protected void _TickDomain(float dt)
        {
            this.debugDomain.Tick();
            this.jumpTextDomain.Tick(dt);
            this.layerDomain.Tick(dt);
        }

        public void Update(float dt)
        {
            var director = this.context.director;
            director.Tick(dt);
            dt *= director.timeScaleCom.timeScale;
            this._PreTick(dt);
            this._Tick(dt);
        }

        public void LateUpdate(float dt)
        {
            this._LateTick(dt);
        }

        protected void _PreTick(float dt)
        {
        }

        protected void _Tick(float dt)
        {
            this._TickDomain(dt);
        }

        protected void _LateTick(float dt)
        {
            this.context.cmdBufferService.Tick();
        }
    }
}