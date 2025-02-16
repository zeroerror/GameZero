using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.UI;
using GamePlay.Core;
using UnityEngine;
namespace GamePlay.Bussiness.Render
{
    public class GameContextR
    {
        public GameDirectorEntityR director { get; private set; }

        public GameDomainApi logicApi { get; private set; }
        public GameCameraEntity cameraEntity { get; private set; }
        public GameDomainApiR domainApi { get; private set; }

        public GameRoleContextR roleContext { get; private set; }
        public GameSkillContextR skillContext { get; private set; }
        public GameActionContextR actionContext { get; private set; }
        public GameVFXContextR vfxContext { get; private set; }
        public GameProjectileContextR projectileContext { get; private set; }
        public GameFieldContextR fieldContext { get; private set; }
        public GameBuffContextR buffContext { get; private set; }
        public GameShaderEffectContext shaderEffectContext { get; private set; }

        public GameEventService eventService { get; private set; }
        public GameEventService delayRCEventService { get; private set; }
        public GameCmdBufferService cmdBufferService { get; private set; }

        public UIDomainApi uiApi { get; private set; }
        public GameContextR()
        {

            var cam = GameObject.Find("Main Camera")?.GetComponent<Camera>();
            this.cameraEntity = new GameCameraEntity(cam);
            this.director = new GameDirectorEntityR();
            this.domainApi = new GameDomainApiR();

            this.roleContext = new GameRoleContextR();
            this.skillContext = new GameSkillContextR();
            this.actionContext = new GameActionContextR();
            this.vfxContext = new GameVFXContextR();
            this.projectileContext = new GameProjectileContextR();
            this.fieldContext = new GameFieldContextR();
            this.buffContext = new GameBuffContextR();
            this.shaderEffectContext = new GameShaderEffectContext();

            this.eventService = new GameEventService();
            this.delayRCEventService = new GameEventService();
            this.cmdBufferService = new GameCmdBufferService();
        }

        public void Inject(GameObject sceneRoot, GameDomainApi logicApi, UIDomainApi uiApi)
        {
            this.fieldContext.Inject(sceneRoot);
            this.logicApi = logicApi;
            this.uiApi = uiApi;
        }

        public void BindRC(string rcName, System.Action<object> callback)
        {
            this.logicApi.directorApi.BindRC(rcName, callback);
            this.delayRCEventService.Bind(rcName, callback);
        }

        public void UnbindRC(string rcName, System.Action<object> callback)
        {
            this.logicApi.directorApi.UnbindRC(rcName, callback);
            this.delayRCEventService.Unbind(rcName, callback);
        }

        public void DelayRC(string rcName, object args)
        {
            this.delayRCEventService.Submit(rcName, args);
        }

        public GameEntityBase FindEntity(in GameIdArgs idArgs)
        {
            switch (idArgs.entityType)
            {
                case GameEntityType.None:
                    return null;
                case GameEntityType.Role:
                    return this.roleContext.repo.FindByEntityId(idArgs.entityId);
                case GameEntityType.Skill:
                    return this.skillContext.repo.FindByEntityId(idArgs.entityId);
                case GameEntityType.Projectile:
                    return this.projectileContext.repo.FindByEntityId(idArgs.entityId);
                case GameEntityType.Buff:
                    return this.buffContext.repo.FindByEntityId(idArgs.entityId);
                default:
                    GameLogger.LogError("GameContextR.FindEntityByEntityId: unknown entityType: " + idArgs.entityType);
                    return null;
            }
        }

        public T FindEntity<T>(in GameIdArgs idArgs) where T : GameEntityBase
        {
            var entity = this.FindEntity(idArgs);
            if (entity == null)
            {
                return null;
            }
            return entity as T;
        }
    }
}