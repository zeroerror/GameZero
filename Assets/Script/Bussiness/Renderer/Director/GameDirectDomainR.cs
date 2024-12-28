using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.UI;
using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectDomainR
    {
        public GameContextR context { get; private set; }
        public GameRoleDomainR roleDomain { get; private set; }
        public GameSkillDomainR skillDomain { get; private set; }
        public GameTransformDomainR transformDomain { get; private set; }
        public GameAttributeDomainR attributeDomain { get; private set; }
        public GameVFXDomainR vfxDomain { get; private set; }
        public GameActionDomainR actionDomain { get; private set; }
        public GameDrawDomainR drawDomain { get; private set; }
        public GameProjectileDomainR projectileDomain { get; private set; }
        public GameFieldDomainR fieldDomain { get; private set; }
        public GameEntityCollectDomainR entityCollectDomain { get; private set; }
        public GameBuffDomainR buffDomain { get; private set; }

        public GameDirectDomainR(GameContext logicContext, GameObject sceneRoot, GameUIContext uiContext)
        {
            this._InitDomain();
            this._InitContext(logicContext, sceneRoot, uiContext);
            this._InjectContext();
        }

        private void _InitDomain()
        {
            this.roleDomain = new GameRoleDomainR();
            this.skillDomain = new GameSkillDomainR();
            this.transformDomain = new GameTransformDomainR();
            this.attributeDomain = new GameAttributeDomainR();
            this.vfxDomain = new GameVFXDomainR();
            this.actionDomain = new GameActionDomainR();
            this.drawDomain = new GameDrawDomainR();
            this.projectileDomain = new GameProjectileDomainR();
            this.fieldDomain = new GameFieldDomainR();
            this.entityCollectDomain = new GameEntityCollectDomainR();
            this.buffDomain = new GameBuffDomainR();
        }

        private void _InitContext(GameContext logicContext, GameObject sceneRoot, GameUIContext uiContext)
        {
            this.context = new GameContextR(logicContext, sceneRoot, uiContext);
            this.context.domainApi.SetRoleApi(this.roleDomain);
            this.context.domainApi.SetSkillApi(this.skillDomain);
            this.context.domainApi.SetTransformApi(this.transformDomain);
            this.context.domainApi.SetAttributeApi(this.attributeDomain);
            this.context.domainApi.SetVFXApi(this.vfxDomain);
            this.context.domainApi.SetActionApi(this.actionDomain);
            this.context.domainApi.SetDrawApi(this.drawDomain);
            this.context.domainApi.SetProjectileApi(this.projectileDomain);
            this.context.domainApi.SetFieldApi(this.fieldDomain);
            this.context.domainApi.SetEntityCollectApi(this.entityCollectDomain);
            this.context.domainApi.SetBuffApi(this.buffDomain);
        }

        private void _InjectContext()
        {
            this._BindEvents();
            this.roleDomain.Inject(this.context);
            this.skillDomain.Inject(this.context);
            this.transformDomain.Inject(this.context);
            this.attributeDomain.Inject(this.context);
            this.vfxDomain.Inject(this.context);
            this.actionDomain.Inject(this.context);
            this.drawDomain.Inject(this.context);
            this.projectileDomain.Inject(this.context);
            this.fieldDomain.Inject(this.context);
            this.entityCollectDomain.Inject(this.context);
            this.buffDomain.Inject(this.context);
        }

        public void Destroy()
        {
            this._UnbindEvents();
            this.roleDomain.Destroy();
            this.skillDomain.Destroy();
            this.transformDomain.Destroy();
            this.attributeDomain.Destroy();
            this.vfxDomain.Destroy();
            this.actionDomain.Destroy();
            this.drawDomain.Destroy();
            this.projectileDomain.Destroy();
            this.fieldDomain.Destroy();
            this.entityCollectDomain.Destroy();
            this.buffDomain.Destroy();
        }

        private void _BindEvents()
        {
            this.context.BindRC(GameDirectRCCollection.RC_DIRECT_TIME_SCALE_CHANGE, this._OnTimeScaleChange);
        }

        private void _UnbindEvents()
        {
            this.context.BindRC(GameDirectRCCollection.RC_DIRECT_TIME_SCALE_CHANGE, this._OnTimeScaleChange);
        }

        private void _OnTimeScaleChange(object args)
        {
            var timeScale = (float)args;
            this.context.director.timeScaleCom.SetTimeScale(timeScale);
        }

        protected virtual void _TickDomain(float dt)
        {
            if (this.context.fieldContext.curField == null) return;
            this.fieldDomain.Tick(dt);
            this.roleDomain.Tick(dt);
            this.skillDomain.Tick(dt);
            this.vfxDomain.Tick(dt);
            this.drawDomain.Tick(dt);
            this.projectileDomain.Tick(dt);
            this.entityCollectDomain.Tick(dt);
            this.buffDomain.Tick(dt);
        }

        public void Update(float dt)
        {
            var director = this.context.director;
            director.Tick(dt);
            dt *= director.timeScaleCom.timeScale;
            this._PreTick(dt);
            this._Tick(dt);
            this._LateTick(dt);
        }

        public void LateUpdate(float dt)
        {
            dt *= this.context.director.timeScaleCom.timeScale;
            this.context.cameraEntity.Tick(dt);
        }

        protected void _PreTick(float dt)
        {
            this._TickDebugInput();
            this.context.delayRCEventService.Tick();
            this.context.logicContext.rcEventService.Tick();
            this.context.eventService.Tick();
        }

        protected void _Tick(float dt)
        {
            this._TickDomain(dt);
        }

        protected void _LateTick(float dt)
        {
            this.context.cmdBufferService.Tick();
        }

        private void _TickDebugInput()
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                var timeScale = this.context.director.timeScaleCom.timeScale;
                timeScale = timeScale == 1.0f ? 0.1f : timeScale == 0.1f ? 0.0f : 1.0f;
                this.context.logicContext.director.timeScaleCom.SetTimeScale(timeScale);
                this.context.director.timeScaleCom.SetTimeScale(timeScale);
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                var timeScale = this.context.director.timeScaleCom.timeScale;
                timeScale = timeScale == 1.0f ? 2.0f : timeScale == 2.0f ? 3.0f : 1.0f;
                this.context.logicContext.director.timeScaleCom.SetTimeScale(timeScale);
                this.context.director.timeScaleCom.SetTimeScale(timeScale);
            }
        }
    }
}