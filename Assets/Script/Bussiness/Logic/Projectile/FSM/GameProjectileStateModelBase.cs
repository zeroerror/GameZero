using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public abstract class GameProjectileStateModelBase
    {
        public float stateTime;
        public int stateFrame => (int)(stateTime * GameTimeCollection.frameRate);

        public GameProjectileStateModelBase()
        {
            stateTime = 0;
        }

        public virtual void Clear()
        {
            stateTime = 0;
        }
    }
}