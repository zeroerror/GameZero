namespace GamePlay.Bussiness.Logic
{
    public abstract class GameRoleStateModelBase
    {
        public float stateTime;

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