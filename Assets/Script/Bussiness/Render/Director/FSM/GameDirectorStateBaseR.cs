namespace GamePlay.Bussiness.Render
{
    public abstract class GameDirectorStateBaseR
    {
        public float stateTime;

        public GameDirectorStateBaseR()
        {
            stateTime = 0;
        }

        public virtual void Clear()
        {
            stateTime = 0;
        }
    }
}