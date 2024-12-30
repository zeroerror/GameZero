using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleContext
    {
        public GameRoleEntity userRole;

        public GameRoleRepo repo { get; private set; }
        public GameRoleFactory factory { get; private set; }
        public GameIdService idService { get; private set; }
        private Dictionary<int, GameRoleInputArgs> _playerInputArgs;
        public List<GameRoleStateRecord> roleStateRecords;

        public GameRoleContext()
        {
            this.repo = new GameRoleRepo();
            this.factory = new GameRoleFactory();
            this.idService = new GameIdService();
            this._playerInputArgs = new Dictionary<int, GameRoleInputArgs>();
            this.roleStateRecords = new List<GameRoleStateRecord>();
        }

        public void ClearPlayerInputArgs()
        {
            this._playerInputArgs.Clear();
        }

        public bool TryGetPlayerInputArgs(int entityId, out GameRoleInputArgs inputArgs)
        {
            return this._playerInputArgs.TryGetValue(entityId, out inputArgs);
        }
        public void SetPlayerInputArgs(int entityId, in GameRoleInputArgs inputArgs)
        {
            if (!this._playerInputArgs.TryGetValue(entityId, out var oldInputArgs))
            {
                this._playerInputArgs[entityId] = inputArgs;
                return;
            }
            oldInputArgs.Update(inputArgs);
            this._playerInputArgs[entityId] = oldInputArgs;
        }
    }
}