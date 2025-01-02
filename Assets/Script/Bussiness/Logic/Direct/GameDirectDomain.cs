using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectDomain
    {
        public GameDirector director => this.context.director;

        public GameContext context { get; private set; }
        public GameRoleDomain roleDomain { get; private set; }
        public GameSkillDomain skillDomain { get; private set; }
        public GameTransformDomain transformDomain { get; private set; }
        public GameAttributeDomain attributeDomain { get; private set; }
        public GamePhysicsDomain physicsDomain { get; private set; }
        public GameActionDomain actionDomain { get; private set; }
        public GameEntitySelectDomain entitySelectDomain { get; private set; }
        public GameProjectileDomain projectileDomain { get; private set; }
        public GameFieldDomain fieldDomain { get; private set; }
        public GameEntityCollectDomain entityCollectDomain { get; private set; }
        public GameBuffDomain buffDomain { get; private set; }

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
            this.attributeDomain = new GameAttributeDomain();
            this.physicsDomain = new GamePhysicsDomain();
            this.actionDomain = new GameActionDomain();
            this.entitySelectDomain = new GameEntitySelectDomain();
            this.projectileDomain = new GameProjectileDomain();
            this.fieldDomain = new GameFieldDomain();
            this.entityCollectDomain = new GameEntityCollectDomain();
            this.buffDomain = new GameBuffDomain();
        }

        private void _InitContext()
        {
            this.context = new GameContext();
            this.context.domainApi.SetRoleApi(this.roleDomain);
            this.context.domainApi.SetSkillApi(this.skillDomain);
            this.context.domainApi.SetTransformApi(this.transformDomain);
            this.context.domainApi.SetAttributeApi(this.attributeDomain);
            this.context.domainApi.SetPhysicsApi(this.physicsDomain);
            this.context.domainApi.SetActionApi(this.actionDomain);
            this.context.domainApi.SetEntitySelectApi(this.entitySelectDomain);
            this.context.domainApi.SetProjectileApi(this.projectileDomain);
            this.context.domainApi.SetFieldApi(this.fieldDomain);
            this.context.domainApi.SetEntityCollectApi(this.entityCollectDomain);
            this.context.domainApi.SetBuffApi(this.buffDomain);
        }

        private void _InjectContext()
        {
            this.roleDomain.Inject(this.context);
            this.skillDomain.Inject(this.context);
            this.transformDomain.Inject(this.context);
            this.attributeDomain.Inject(this.context);
            this.physicsDomain.Inject(this.context);
            this.actionDomain.Inject(this.context);
            this.entitySelectDomain.Inject(this.context);
            this.projectileDomain.Inject(this.context);
            this.fieldDomain.Inject(this.context);
            this.entityCollectDomain.Inject(this.context);
            this.buffDomain.Inject(this.context);

            // TEST
            this.roleDomain.CreatePlayerRole(1001, new GameTransformArgs { position = new GameVec2(0, -5), scale = GameVec2.one, forward = GameVec2.right }, true);
            this.fieldDomain.LoadField(1);
        }

        public void Destroy()
        {
            this.roleDomain.Destroy();
            this.skillDomain.Destroy();
            this.transformDomain.Destroy();
            this.attributeDomain.Destroy();
            this.physicsDomain.Destroy();
            this.actionDomain.Destroy();
            this.entitySelectDomain.Destroy();
            this.projectileDomain.Destroy();
            this.fieldDomain.Destroy();
            this.entityCollectDomain.Destroy();
            this.buffDomain.Destroy();
        }

        protected virtual void _TickDomain(float dt)
        {
            this.fieldDomain.Tick(dt);
            if (this.context.fieldContext.curField == null) return;
            this.roleDomain.Tick(dt);
            this.skillDomain.Tick(dt);
            this.transformDomain.Tick(dt);
            this.attributeDomain.Tick(dt);
            this.entitySelectDomain.Tick(dt);
            this.projectileDomain.Tick(dt);
            this.entityCollectDomain.Tick(dt);
            this.buffDomain.Tick(dt);
            this.actionDomain.Tick(dt);
        }

        public void Update(float dt)
        {
            var tickCount = this.director.Tick(dt);
            if (tickCount <= 0) return;
            var frameTime = GameTimeCollection.frameTime;
            for (var i = 0; i < tickCount; i++)
            {
                this._PreTick(frameTime);
                this._Tick(frameTime);
                this._LateTick(dt);
            }
        }

        protected void _PreTick(float dt)
        {
            if (this.director.timeScaleCom.timeScaleDirty)
            {
                this.director.timeScaleCom.ClearTimeScaleDirty();
                this.context.SubmitRC(GameDirectRCCollection.RC_DIRECT_TIME_SCALE_CHANGE, this.director.timeScaleCom.timeScale);
            }
            this.context.eventService.Tick();
        }

        protected void _Tick(float dt)
        {
            this._TickDomain(dt);
        }

        protected void _LateTick(float dt)
        {
            this.physicsDomain.Tick(dt);
            this.context.cmdBufferService.Tick();
        }
    }
}