using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameDirectorEntityR
    {
        public GameDirectorFSMCom fsmCom { get; private set; } = new GameDirectorFSMCom();
        public GameDirectorTimelineComponent timeScaleCom { get; private set; } = new GameDirectorTimelineComponent();

        public void Tick(float dt)
        {
            this.timeScaleCom.Tick(dt);
        }
    }
}