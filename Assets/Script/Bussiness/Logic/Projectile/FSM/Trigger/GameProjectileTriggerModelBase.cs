namespace GamePlay.Bussiness.Logic
{
    public abstract class GameProjectileTriggerModelBase
    {
        public readonly int actionId;
        public readonly GameProjectileStateType nextStateType;

        public GameProjectileTriggerModelBase(int actionId, GameProjectileStateType nextStateType)
        {
            this.actionId = actionId;
            this.nextStateType = nextStateType;
        }
    }
}