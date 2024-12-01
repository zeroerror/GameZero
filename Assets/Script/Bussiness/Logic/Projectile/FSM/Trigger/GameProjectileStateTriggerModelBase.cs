namespace GamePlay.Bussiness.Logic
{
    public abstract class GameProjectileStateTriggerModelBase
    {
        public readonly int actionId;
        public readonly GameProjectileStateType nextStateType;

        public GameProjectileStateTriggerModelBase(int actionId, GameProjectileStateType nextStateType)
        {
            this.actionId = actionId;
            this.nextStateType = nextStateType;
        }
    }
}