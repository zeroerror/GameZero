namespace GamePlay.Bussiness.Logic
{
    public class GameDirector
    {
        public GameDirectorFSMCom fsmCom { get; private set; } = new GameDirectorFSMCom();
        public GameDirectorTimelineComponent timeScaleCom { get; private set; } = new GameDirectorTimelineComponent();

        public int Tick(float dt)
        {
            var tickCount = this.timeScaleCom.Tick(dt);
            return tickCount;
        }
    }
}