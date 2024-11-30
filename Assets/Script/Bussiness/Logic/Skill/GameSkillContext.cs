using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameSkillContext
    {
        public GameSkillFactory factory { get; private set; }
        public GameIdService entityIdService { get; private set; }
        public GameSkillRepo repo { get; private set; }

        public GameSkillContext()
        {
            factory = new GameSkillFactory();
            entityIdService = new GameIdService();
            repo = new GameSkillRepo();
        }
    }
}