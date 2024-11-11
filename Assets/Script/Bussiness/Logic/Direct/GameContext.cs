using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameContext
    {
        public GameDirector director { get; private set; } = new GameDirector();
        public GameEventService eventService { get; private set; } = new GameEventService();
        public GameContext()
        {
        }
    }
}