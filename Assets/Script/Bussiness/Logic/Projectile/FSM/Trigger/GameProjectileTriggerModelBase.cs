namespace GamePlay.Bussiness.Logic
{
    public abstract class GameProjectileTriggerModelBase
    {
        public readonly int[] actionIds;
        public readonly GameProjectileStateType nextStateType;

        public GameProjectileTriggerModelBase(int[] actionIds, GameProjectileStateType nextStateType)
        {
            this.actionIds = actionIds;
            this.nextStateType = nextStateType;
        }
    }
}