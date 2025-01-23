using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorEntity
    {
        public GameDirectorFSMCom fsmCom { get; private set; } = new GameDirectorFSMCom();
        public GameDirectorTimelineComponent timeScaleCom { get; private set; } = new GameDirectorTimelineComponent();

        public int coins;
        public List<GameActionOptionModel> actionOptions = new List<GameActionOptionModel>();

        public int Tick(float dt)
        {
            var tickCount = this.timeScaleCom.Tick(dt);
            return tickCount;
        }
    }
}