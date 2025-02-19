using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public class GameSkillContext
    {
        public GameSkillFactory factory { get; private set; }
        public GameIdService idService { get; private set; }
        public GameSkillRepo repo { get; private set; }

        public GameSkillContext()
        {
            factory = new GameSkillFactory();
            idService = new GameIdService();
            repo = new GameSkillRepo();
        }
    }
}