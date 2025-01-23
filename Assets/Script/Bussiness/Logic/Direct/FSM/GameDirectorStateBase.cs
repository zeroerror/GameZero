namespace GamePlay.Bussiness.Logic
{
    public abstract class GameDirectorStateBase
    {
        public float stateTime;
        public int stateFrame => (int)(stateTime * GameTimeCollection.frameRate);

        public GameDirectorStateBase()
        {
            stateTime = 0;
        }

        public virtual void Clear()
        {
            stateTime = 0;
        }
    }
}