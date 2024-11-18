using System;
using System.Collections.Generic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleContext
    {
        public GameRoleEntityRepo entityRepo { get; private set; }
        public GameRoleFactory factory { get; private set; }
        public GameIdService entityIdService { get; private set; }
        private Dictionary<int, GameRoleInputArgs> _playerInputArgs;
        public GameRoleEntity userRole;

        public GameRoleContext()
        {
            this.entityRepo = new GameRoleEntityRepo();
            this.factory = new GameRoleFactory();
            this.entityIdService = new GameIdService();
            this._playerInputArgs = new Dictionary<int, GameRoleInputArgs>();
        }

        public void ClearPlayerInputArgs()
        {
            this._playerInputArgs.Clear();
        }

        public void ForeachPlayerInputArgs(System.Action<int, GameRoleInputArgs> action)
        {
            foreach (var item in this._playerInputArgs)
            {
                action(item.Key, item.Value);
            }
        }

        public void SetPlayerInputArgs(int entityId, in GameRoleInputArgs inputArgs)
        {
            this._playerInputArgs[entityId] = inputArgs;
        }
    }
}