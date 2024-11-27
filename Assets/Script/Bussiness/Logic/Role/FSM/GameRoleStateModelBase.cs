namespace GamePlay.Bussiness.Logic
{
    public abstract class GameRoleStateModelBase
    {
        public float stateTime;
        public int stateFrame => (int)(stateTime * GameTimeCollection.frameRate);

        public GameRoleStateModelBase()
        {
            stateTime = 0;
        }

        public virtual void Clear()
        {
            stateTime = 0;
        }
    }
}