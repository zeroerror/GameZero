namespace GamePlay.Bussiness.Logic
{
    public class GameSkillContext
    {
        public GameSkillFactory factory { get; private set; }

        public GameSkillContext()
        {
            factory = new GameSkillFactory();
        }
    }
}