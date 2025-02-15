using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GamePhysicsContext
    {
        public GameIdService idService { get; private set; } = new GameIdService();
        public List<GamePhysicsCom> physicsComs { get; private set; } = new List<GamePhysicsCom>();
        public List<GamePhysicsCom> physicsComs_notTrigger { get; private set; } = new List<GamePhysicsCom>();

        public GamePhysicsContext() { }

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