using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerCom
    {
        public Dictionary<GameProjectileStateType, GameProjectileStateTriggerModelSet> triggerSetDict { get; private set; }

        public GameProjectileStateTriggerCom()
        {
            this.triggerSetDict = new Dictionary<GameProjectileStateType, GameProjectileStateTriggerModelSet>();
        }
    }
}