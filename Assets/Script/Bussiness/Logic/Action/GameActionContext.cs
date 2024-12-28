using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionContext
    {
        public GameActionTemplate template { get; private set; }

        public List<GameActionRecord_Dmg> dmgRecordList { get; private set; }
        public List<GameActionRecord_Heal> healRecordList { get; private set; }
        public List<GameActionRecord_LaunchProjectile> launchProjectileRecordList { get; private set; }

        public GameActionContext()
        {
            this.template = new GameActionTemplate();
            this.dmgRecordList = new List<GameActionRecord_Dmg>();
            this.healRecordList = new List<GameActionRecord_Heal>();
            this.launchProjectileRecordList = new List<GameActionRecord_LaunchProjectile>();
        }
    }
}