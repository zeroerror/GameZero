namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerModel_VolumeCollision : GameProjectileTriggerModelBase
    {
        public readonly GameEntitySelector detectEntitySelector;

        public GameProjectileTriggerModel_VolumeCollision(
            int actionId,
            GameProjectileStateType nextStateType,
            GameEntitySelector detectEntitySelector
        ) : base(actionId, nextStateType)
        {
            this.detectEntitySelector = detectEntitySelector;
        }

        public void Clear()
        {
        }
    }
}