using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameContext
    {
        public GameDomainApi domainApi { get; private set; }
        public GameDirector director { get; private set; }
        public GameEventService eventService { get; private set; }
        public GameEventService rcEventService { get; private set; }
        public GameRoleContext roleContext { get; private set; }
        public GameSkillContext skillContext { get; private set; }
        public GamePhysicsContext physicsContext { get; private set; }
        public GameActionContext actionContext { get; private set; }
        public GameContext()
        {
            this.domainApi = new GameDomainApi();
            this.director = new GameDirector();
            this.eventService = new GameEventService();
            this.rcEventService = new GameEventService();
            this.roleContext = new GameRoleContext();
            this.skillContext = new GameSkillContext();
            this.physicsContext = new GamePhysicsContext();
            this.actionContext = new GameActionContext();
        }

        public void SubmitRC(string name, object args)
        {
            this.rcEventService.Submit(name, args);
        }

        public void BindRC(string rcName, System.Action<object> callback)
        {
            this.rcEventService.Bind(rcName, callback);
        }
    }
}