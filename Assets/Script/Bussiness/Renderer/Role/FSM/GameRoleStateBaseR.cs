namespace GamePlay.Bussiness.Render
{
    public abstract class GameRoleStateBaseR
    {
        public float stateTime;
        public GameRoleStateBaseR()
        {
            stateTime = 0;
        }
        public virtual void Clear()
        {
            stateTime = 0;
        }
    }
}