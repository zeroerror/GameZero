using System;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorDomain : GameDirectorDomainApi
    {
        public GameDirectorEntity director => this.context.director;

        public GameDirectorFSMDomain fsmDomain { get; private set; }
        public GameDirectorFSMDomainApi fsmApi => this.fsmDomain;

        public GameContext context { get; private set; }
        public GameFieldDomain fieldDomain { get; private set; }
        public GameRoleDomain roleDomain { get; private set; }
        public GameSkillDomain skillDomain { get; private set; }
        public GameBuffDomain buffDomain { get; private set; }
        public GameProjectileDomain projectileDomain { get; private set; }
        public GameActionDomain actionDomain { get; private set; }
        public GameTransformDomain transformDomain { get; private set; }
        public GameAttributeDomain attributeDomain { get; private set; }
        public GamePhysicsDomain physicsDomain { get; private set; }
        public GameEntitySelectDomain entitySelectDomain { get; private set; }
        public GameEntityCollectDomain entityCollectDomain { get; private set; }

        public GameDirectorDomain()
        {
            this._InitDomain();
            this._InitContext();
            this._InjectContext();
        }

        private void _InitDomain()
        {
            this.fieldDomain = new GameFieldDomain();
            this.roleDomain = new GameRoleDomain();
            this.skillDomain = new GameSkillDomain();
            this.buffDomain = new GameBuffDomain();
            this.projectileDomain = new GameProjectileDomain();
            this.actionDomain = new GameActionDomain();
            this.transformDomain = new GameTransformDomain();
            this.attributeDomain = new GameAttributeDomain();
            this.physicsDomain = new GamePhysicsDomain();
            this.entitySelectDomain = new GameEntitySelectDomain();
            this.entityCollectDomain = new GameEntityCollectDomain();
            this.fsmDomain = new GameDirectorFSMDomain(this);
        }

        private void _InitContext()
        {
            this.context = new GameContext();
            this.context.domainApi.SetDirectApi(this);
            this.context.domainApi.SetFieldApi(this.fieldDomain);
            this.context.domainApi.SetRoleApi(this.roleDomain);
            this.context.domainApi.SetSkillApi(this.skillDomain);
            this.context.domainApi.SetBuffApi(this.buffDomain);
            this.context.domainApi.SetProjectileApi(this.projectileDomain);
            this.context.domainApi.SetActionApi(this.actionDomain);
            this.context.domainApi.SetTransformApi(this.transformDomain);
            this.context.domainApi.SetAttributeApi(this.attributeDomain);
            this.context.domainApi.SetPhysicsApi(this.physicsDomain);
            this.context.domainApi.SetEntitySelectApi(this.entitySelectDomain);
            this.context.domainApi.SetEntityCollectApi(this.entityCollectDomain);
        }

        private void _InjectContext()
        {
            this.fsmDomain.Inject(this.context);
            this.fieldDomain.Inject(this.context);
            this.roleDomain.Inject(this.context);
            this.skillDomain.Inject(this.context);
            this.buffDomain.Inject(this.context);
            this.projectileDomain.Inject(this.context);
            this.actionDomain.Inject(this.context);
            this.transformDomain.Inject(this.context);
            this.attributeDomain.Inject(this.context);
            this.physicsDomain.Inject(this.context);
            this.entitySelectDomain.Inject(this.context);
            this.entityCollectDomain.Inject(this.context);

            // TEST
            this.fieldDomain.LoadField(1);
            this.roleDomain.CreatePlayerRole(101, new GameTransformArgs { position = new GameVec2(0, -5), scale = GameVec2.one, forward = GameVec2.right }, true);
            this.fsmDomain.TryEnter(this.director, GameDirectorStateType.Fighting);
        }

        public void Destroy()
        {
            this.fsmDomain.Destroy();
            this.fieldDomain.Destroy();
            this.roleDomain.Destroy();
            this.skillDomain.Destroy();
            this.buffDomain.Destroy();
            this.projectileDomain.Destroy();
            this.actionDomain.Destroy();
            this.transformDomain.Destroy();
            this.attributeDomain.Destroy();
            this.physicsDomain.Destroy();
            this.entitySelectDomain.Destroy();
            this.entityCollectDomain.Destroy();
        }

        public virtual void TickDomain(float dt)
        {
            this.fieldDomain.Tick(dt);
            if (this.context.fieldContext.curField == null) return;
            this.roleDomain.Tick(dt);
            this.skillDomain.Tick(dt);
            this.buffDomain.Tick(dt);
            this.projectileDomain.Tick(dt);
            this.actionDomain.Tick(dt);
            this.transformDomain.Tick(dt);
            this.attributeDomain.Tick(dt);
            this.physicsDomain.Tick(dt);
            this.entitySelectDomain.Tick(dt);
            this.entityCollectDomain.Tick(dt);
            this.physicsDomain.Tick(dt);
        }

        public void Update(float dt)
        {
            var tickCount = this.director.Tick(dt);
            if (tickCount <= 0) return;
            var frameTime = GameTimeCollection.frameTime;
            for (var i = 0; i < tickCount; i++)
            {
                this.context.eventService.Tick();
                this.fsmDomain.Tick(this.director, frameTime);
                this.context.cmdBufferService.Tick();
            }
        }

        public void SetTimeScale(float timeScale)
        {
            this.director.timeScaleCom.SetTimeScale(timeScale);
        }

        public void TickRCEvents()
        {
            this.context.rcEventService.Tick();
        }

        public void BindRC(string rcName, Action<object> callback)
        {
            this.context.BindRC(rcName, callback);
        }

        public void UnbindRC(string rcName, Action<object> callback)
        {
            this.context.UnbindRC(rcName, callback);
        }

        public void SubmitEvent(string eventName, object args)
        {
            this.context.eventService.Submit(eventName, args);
        }
    }

}