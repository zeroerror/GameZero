namespace GamePlay.Bussiness.Logic
{
    public class GameDirectDomain
    {
        public GameContext context { get; private set; }

        public GameDirector director => this.context.director;

        public GameTransformDomain transformDomain { get; private set; }
        public GameRoleDomain roleDomain { get; private set; }

        public GameDirectDomain()
        {
            this._InitDomain();
            this._InitContext();
            this._InjectContext();
        }

        private void _InitDomain()
        {
            this.roleDomain = new GameRoleDomain();
            this.transformDomain = new GameTransformDomain();
        }

        private void _InitContext()
        {
            this.context = new GameContext();
            this.context.domainApi.SetRoleApi(this.roleDomain);
            this.context.domainApi.SetTransformApi(this.transformDomain);
        }

        private void _InjectContext()
        {
            this.roleDomain.Inject(this.context);
            this.transformDomain.Inject(this.context);
        }

        public void Dispose()
        {
            this.roleDomain.Dispose();
            this.transformDomain.Dispose();
        }

        public void Update(float dt)
        {
            var canTick = this.director.Tick(dt);
            if (!canTick) return;
            var frameTime = this.director.timeScaleCom.frameTime;
            this._PreTick(frameTime);
            this._Tick(frameTime);
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
            this._TickDomain(dt);
        }

        protected void _LateTick(float dt)
        {
        }

        protected virtual void _TickDomain(float dt)
        {
            this.roleDomain.Tick(dt);
            this.transformDomain.Tick(dt);
        }
    }
}