using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public abstract class GameProjectileStateBase
    {
        public float stateTime;
        public int stateFrame => (int)(stateTime * GameTimeCollection.frameRate);

        public GameProjectileStateBase()
        {
            stateTime = 0;
        }

        public virtual void Clear()
        {
            stateTime = 0;
        }
    }
}