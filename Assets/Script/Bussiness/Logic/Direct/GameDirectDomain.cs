namespace GamePlay.Bussiness.Logic
{
    public class GameDirectDomain
    {
        public GameContext context { get; private set; }

        public GameDirector director => this.context.director;

        public GameRoleDomain roleDomain { get; private set; }

        public GameDirectDomain()
        {
            this.context = new GameContext();
            this.roleDomain = new GameRoleDomain(this.context);
        }

        public void Dispose()
        {
            this.roleDomain.Dispose();
        }

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
            this.context.rcEventService.Tick();
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
        }
    }
}