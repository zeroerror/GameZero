using GamePlay.Bussiness.Logic;
using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectDomainR
    {
        public GameContextR context { get; private set; }
        public GameRoleDomainR roleDomain { get; private set; }
        public GameSkillDomainR skillDomain { get; private set; }
        public TransformDomainR transformDomain { get; private set; }
        public GameVFXDomainR vfxDomain { get; private set; }
        public GameActionDomainR actionDomain { get; private set; }
        public GameDrawDomainR drawDomain { get; private set; }
        public GameProjectileDomainR projectileDomain { get; private set; }

        public GameDirectDomainR(GameContext logicContext)
        {
            this._InitDomain();
            this._InitContext(logicContext);
            this._InjectContext();
        }

        private void _InitDomain()
        {
            this.roleDomain = new GameRoleDomainR();
            this.skillDomain = new GameSkillDomainR();
            this.transformDomain = new TransformDomainR();
            this.vfxDomain = new GameVFXDomainR();
            this.actionDomain = new GameActionDomainR();
            this.drawDomain = new GameDrawDomainR();
            this.projectileDomain = new GameProjectileDomainR();
        }

        private void _InitContext(GameContext logicContext)
        {
            this.context = new GameContextR(logicContext);
            this.context.domainApi.SetRoleApi(this.roleDomain);
            this.context.domainApi.SetSkillApi(this.skillDomain);
            this.context.domainApi.SetTransformApi(this.transformDomain);
            this.context.domainApi.SetVFXApi(this.vfxDomain);
            this.context.domainApi.SetActionApi(this.actionDomain);
            this.context.domainApi.SetDrawApi(this.drawDomain);
            this.context.domainApi.SetProjectileApi(this.projectileDomain);

        }

        private void _InjectContext()
        {
            this.roleDomain.Inject(this.context);
            this.skillDomain.Inject(this.context);
            this.transformDomain.Inject(this.context);
            this.vfxDomain.Inject(this.context);
            this.actionDomain.Inject(this.context);
            this.drawDomain.Inject(this.context);
            this.projectileDomain.Inject(this.context);
        }

        public void Dispose()
        {
            this.roleDomain.Dispose();
            this.skillDomain.Dispose();
            this.transformDomain.Dispose();
            this.vfxDomain.Dispose();
            this.actionDomain.Dispose();
            this.drawDomain.Dispose();
            this.projectileDomain.Dispose();
        }

        protected virtual void _TickDomain(float dt)
        {
            this.roleDomain.Tick(dt);
            this.skillDomain.Tick(dt);
            this.vfxDomain.Tick(dt);
            this.drawDomain.Tick(dt);
            this.projectileDomain.Tick(dt);
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
            this.context.cameraEntity.Tick(dt);
        }
    }
}