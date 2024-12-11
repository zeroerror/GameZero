using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GamePhysicsContext
    {
        public GameIdService idService { get; private set; }
        public List<GamePhysicsCom> physicsComs { get; private set; }
        public List<GamePhysicsCom> physicsComs_notTrigger { get; private set; }

        public GamePhysicsContext()
        {
            this.idService = new GameIdService();
            this.physicsComs = new List<GamePhysicsCom>();
            this.physicsComs_notTrigger = new List<GamePhysicsCom>();
        }

        public void ForeachAll(System.Action<GamePhysicsCom> action)
        {
            this.Foreach_NotTrigger(action);
            this.Foreach_Trigger(action);
        }

        public void Foreach_NotTrigger(System.Action<GamePhysicsCom> action)
        {
            foreach (var physicsCom in physicsComs_notTrigger) action(physicsCom);
        }

        public void Foreach_Trigger(System.Action<GamePhysicsCom> action)
        {
            foreach (var physicsCom in physicsComs) action(physicsCom);
        }

        public void Add(GamePhysicsCom physicsCom)
        {
            if (this.physicsComs.Contains(physicsCom)) return;
            this.physicsComs.Add(physicsCom);
            if (!physicsCom.collider.isTrigger)
            {
                this.physicsComs_notTrigger.Add(physicsCom);
            }
        }

        public void Remove(GamePhysicsCom physicsCom)
        {
            if (!this.physicsComs.Contains(physicsCom)) return;
            this.physicsComs.Remove(physicsCom);
            if (!physicsCom.collider.isTrigger)
            {
                this.physicsComs_notTrigger.Remove(physicsCom);
            }
        }
    }
}