namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerModel_VolumeCollision : GameProjectileTriggerModelBase
    {
        public readonly GameEntitySelector detectEntitySelector;

        public GameProjectileTriggerModel_VolumeCollision(
            int[] actionIds,
            GameProjectileStateType nextStateType,
            GameEntitySelector detectEntitySelector
        ) : base(actionIds, nextStateType)
        {
            this.detectEntitySelector = detectEntitySelector;
        }

        public void Clear()
        {
        }
    }
}