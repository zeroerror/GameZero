using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public abstract class GameProjectileStateModelBase
    {
        public float stateTime;
        public int stateFrame => (int)(stateTime * GameTimeCollection.frameRate);

        public GameProjectileStateTriggerModelSet triggerSet;

        public GameProjectileStateModelBase()
        {
            stateTime = 0;
            triggerSet = new GameProjectileStateTriggerModelSet();
        }

        public virtual void Clear()
        {
            stateTime = 0;
        }
    }
}