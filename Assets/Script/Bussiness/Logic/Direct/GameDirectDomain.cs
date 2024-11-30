using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectDomain
    {
        public GameContext context { get; private set; }

        public GameDirector director => this.context.director;

        public GameRoleDomain roleDomain { get; private set; }
        public GameSkillDomain skillDomain { get; private set; }
        public GameTransformDomain transformDomain { get; private set; }
        public GamePhysicsDomain physicsDomain { get; private set; }
        public GameActionDomain actionDomain { get; private set; }
        public GameEntitySelectDomain entitySelectDomain { get; private set; }

        public GameDirectDomain()
        {
            this._InitDomain();
            this._InitContext();
            this._InjectContext();
        }

        private void _InitDomain()
        {
            this.roleDomain = new GameRoleDomain();
            this.skillDomain = new GameSkillDomain();
            this.transformDomain = new GameTransformDomain();
            this.physicsDomain = new GamePhysicsDomain();
            this.actionDomain = new GameActionDomain();
            this.entitySelectDomain = new GameEntitySelectDomain();
        }

        private void _InitContext()
        {
            this.context = new GameContext();
            this.context.domainApi.SetRoleApi(this.roleDomain);
            this.context.domainApi.SetSkillApi(this.skillDomain);
            this.context.domainApi.SetTransformApi(this.transformDomain);
            this.context.domainApi.SetPhysicsApi(this.physicsDomain);
            this.context.domainApi.SetActionApi(this.actionDomain);
            this.context.domainApi.SetEntitySelectApi(this.entitySelectDomain);
        }

        private void _InjectContext()
        {
            this.roleDomain.Inject(this.context);
            this.skillDomain.Inject(this.context);
            this.transformDomain.Inject(this.context);
            this.physicsDomain.Inject(this.context);
            this.actionDomain.Inject(this.context);
            this.entitySelectDomain.Inject(this.context);

            this.roleDomain.CreateUserRole(1001, 1, new GameTransformArgs { position = new GameVec2(-5, -5), scale = 1f });
            this.roleDomain.CreateUserRole(1001, 2, new GameTransformArgs { position = new GameVec2(0, -5), scale = 1f });
            this.roleDomain.CreateUserRole(1001, 3, new GameTransformArgs { position = new GameVec2(5, -5), scale = 1f });
        }

        public void Dispose()
        {
            this.roleDomain.Dispose();
            this.skillDomain.Dispose();
            this.transformDomain.Dispose();
            this.physicsDomain.Dispose();
        }

        public void Update(float dt)
        {
            var canTick = this.director.Tick(dt);
            if (!canTick) return;
            var frameTime = GameTimeCollection.frameTime;
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
            this.physicsDomain.Tick(dt);
        }

        protected virtual void _TickDomain(float dt)
        {
            this.roleDomain.Tick(dt);
            this.transformDomain.Tick(dt);
        }
    }
}