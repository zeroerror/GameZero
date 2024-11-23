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

        public GameContextR(GameContext logicContext)
        {
            this.logicContext = logicContext;
            this.domainApi = new GameDomainApiR();
            this.cameraEntity = new GameCameraEntity(GameObject.Find("Main Camera")?.GetComponent<Camera>());
            this.director = new GameDirectorR();
            this.eventService = new GameEventService();
            this.delayRCEventService = new GameEventService();
            this.roleContext = new GameRoleContextR(this);
        }

        public void RegistRC(string rcName, System.Action<object> callback)
        {
            this.logicContext.rcEventService.Regist(rcName, callback);
            this.delayRCEventService.Regist(rcName, callback);
        }

        public void UnbindRC(string rcName, System.Action<object> callback)
        {
            this.logicContext.rcEventService.Unbind(rcName, callback);
            this.delayRCEventService.Unbind(rcName, callback);
        }
    }
}