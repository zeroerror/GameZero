using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionContext
    {
        public GameActionTemplate template { get; private set; }

        public List<GameActionRecord> recordList { get; private set; }

        public GameActionContext()
        {
        }

        public void AddRecord(GameActionRecord record)
        {
            this.recordList.Add(record);
        }
    }
}