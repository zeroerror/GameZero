using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class GameUIDirectDomain
    {
        public GameUIContext context { get; private set; }
        public GameUIJumpTextDomain jumpTextDomain { get; private set; }

        public GameUIDirectDomain(GameObject uiRoot)
        {
            this._InitDomain();
            this._InitContext(uiRoot);
            this._InjectContext(uiRoot);
        }

        private void _InitDomain()
        {
            this.jumpTextDomain = new GameUIJumpTextDomain();
        }

        private void _InitContext(GameObject uiRoot)
        {
            this.context = new GameUIContext(uiRoot);
            var domainApi = this.context.domainApi;
            domainApi.SetJumpTextDomainApi(this.jumpTextDomain);
        }

        private void _InjectContext(GameObject uiRoot)
        {
            this.jumpTextDomain.Inject(this.context);
        }

        public void Destroy()
        {
            this.jumpTextDomain.Destroy();
        }

        protected void _TickDomain(float dt)
        {
            this.jumpTextDomain.Tick(dt);
        }

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