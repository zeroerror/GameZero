namespace GamePlay.Bussiness.Renderer
{
    public abstract class GameRoleStateModelBaseR
    {
        public float stateTime;
        public GameRoleStateModelBaseR()
        {
            stateTime = 0;
        }
        public virtual void Clear()
        {
            stateTime = 0;
        }
    }
}