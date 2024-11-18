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

        public GameContext()
        {
            this.domainApi = new GameDomainApi();
            this.director = new GameDirector();
            this.eventService = new GameEventService();
            this.rcEventService = new GameEventService();
            this.roleContext = new GameRoleContext();
        }
    }
}