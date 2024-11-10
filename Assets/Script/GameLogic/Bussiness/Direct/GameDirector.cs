namespace GamePlay.Logic
{
    public class GameDirector
    {
        public GameDirectorFSMCom fsmCom { get; private set; } = new GameDirectorFSMCom();

        public GameDirectorTimelineComponent timeScaleCom { get; private set; } = new GameDirectorTimelineComponent();


        public bool Tick(float dt)
        {
            this.fsmCom.Tick(dt);
            var canTickNext = this.timeScaleCom.Tick(dt);
            return canTickNext;
        }
    }
}