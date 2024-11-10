namespace GamePlay.Logic
{
    public class GameContext
    {
        public GameDirector director { get; private set; } = new GameDirector();
        public GameContext()
        {
        }
    }
}