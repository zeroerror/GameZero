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