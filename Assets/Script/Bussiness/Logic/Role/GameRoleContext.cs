using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleContext
    {
        public GameRoleEntity userRole;
        public Dictionary<int, GameRoleInputArgs> playerInputArgs { get; private set; }

        public GameRoleRepo repo { get; private set; }
        public GameRoleFactory factory { get; private set; }
        public GameIdService idService { get; private set; }


        public List<GameRoleStateRecord> roleStateRecords;

        public GameRoleContext()
        {
            this.repo = new GameRoleRepo();
            this.factory = new GameRoleFactory();
            this.idService = new GameIdService();
            this.playerInputArgs = new Dictionary<int, GameRoleInputArgs>();
            this.roleStateRecords = new List<GameRoleStateRecord>();
        }

        public void ClearPlayerInputArgs()
        {
            this.playerInputArgs.Clear();
        }

        public void ClearRoleStateRecords()
        {
            this.roleStateRecords.Clear();
        }
    }
}