namespace GamePlay.Bussiness.Logic
{
    public abstract class GameRoleStateBase
    {
        public float stateTime;
        public int stateFrame => (int)(stateTime * GameTimeCollection.frameRate);

        public GameRoleStateBase()
        {
            stateTime = 0;
        }

        public virtual void Clear()
        {
            stateTime = 0;
        }
    }
}