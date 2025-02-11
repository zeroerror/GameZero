using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Render
{
    public class GameDirectorEntityR
    {
        public GameDirectorFSMCom fsmCom { get; private set; } = new GameDirectorFSMCom();
        public GameDirectorTimelineComponent timeScaleCom { get; private set; } = new GameDirectorTimelineComponent();

        public int curRound;

        public void Tick(float dt)
        {
            this.timeScaleCom.Tick(dt);
        }
    }
}