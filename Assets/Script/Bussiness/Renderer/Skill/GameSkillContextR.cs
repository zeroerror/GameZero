using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Renderer
{
    public class GameSkillContextR
    {
        public GameSkillFactoryR factory { get; private set; }

        public GameSkillContextR()
        {
            factory = new GameSkillFactoryR();
        }
    }
}