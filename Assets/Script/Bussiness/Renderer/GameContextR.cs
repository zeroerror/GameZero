using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameContextR
    {
        public GameContext logicContext { get; private set; }
        public GameDomainApiR domainApi { get; private set; }

        public GameCameraEntity cameraEntity { get; private set; }
        public GameDirectorR director { get; private set; }
        public GameEventService eventService { get; private set; }
        public GameEventService delayRCEventService { get; private set; }
        public GameRoleContextR roleContext { get; private set; }
        public GameSkillContextR skillContext { get; private set; }
        public GameActionContextR actionContext { get; private set; }
        public GameVFXContextR vfxContext { get; private set; }

        public GameContextR(GameContext logicContext)
        {
            this.logicContext = logicContext;
            this.domainApi = new GameDomainApiR();
            this.cameraEntity = new GameCameraEntity(GameObject.Find("Main Camera")?.GetComponent<Camera>());
            this.director = new GameDirectorR();
            this.eventService = new GameEventService();
            this.delayRCEventService = new GameEventService();
            this.roleContext = new GameRoleContextR();
            this.skillContext = new GameSkillContextR();
            this.actionContext = new GameActionContextR();
            this.vfxContext = new GameVFXContextR();
        }

        public void BindRC(string rcName, System.Action<object> callback)
        {
            this.logicContext.BindRC(rcName, callback);
            this.delayRCEventService.Bind(rcName, callback);
        }

        public void UnbindRC(string rcName, System.Action<object> callback)
        {
            this.logicContext.rcEventService.Unbind(rcName, callback);
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
                case GameEntityType.Role:
                    return this.roleContext.repo.FindByEntityId(idArgs.entityId);
                default:
                    GameLogger.LogError("GameContextR.FindEntityByEntityId: unknown entityType: " + idArgs.entityType);
                    return null;
            }
        }
    }
}