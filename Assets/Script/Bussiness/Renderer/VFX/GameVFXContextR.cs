using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameVFXContextR
    {
        public GameVFXRepoR repo => this._repo;
        GameVFXRepoR _repo;
        public GameVFXFactoryR factory => this._factory;
        GameVFXFactoryR _factory;

        public GameIdService entityIdService { get; private set; }

        public GameVFXContextR()
        {
            this._repo = new GameVFXRepoR();
            this._factory = new GameVFXFactoryR();
            this.entityIdService = new GameIdService();
        }
    }
}