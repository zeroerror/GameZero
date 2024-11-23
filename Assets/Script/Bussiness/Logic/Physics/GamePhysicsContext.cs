using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GamePhysicsContext
    {
        public GameIdService idService { get; private set; }
        public List<GamePhysicsCom> physicsComs { get; private set; }

        public GamePhysicsContext()
        {
            this.idService = new GameIdService();
            this.physicsComs = new List<GamePhysicsCom>();
        }

        public void Foreach(System.Action<GamePhysicsCom> action)
        {
            foreach (var physicsCom in physicsComs)
            {
                action(physicsCom);
            }
        }

        public void Add(GamePhysicsCom physicsCom)
        {
            if (this.physicsComs.Contains(physicsCom)) return;
            this.physicsComs.Add(physicsCom);
        }

        public void Remove(GamePhysicsCom physicsCom)
        {
            if (!this.physicsComs.Contains(physicsCom)) return;
            this.physicsComs.Remove(physicsCom);
        }
    }
}