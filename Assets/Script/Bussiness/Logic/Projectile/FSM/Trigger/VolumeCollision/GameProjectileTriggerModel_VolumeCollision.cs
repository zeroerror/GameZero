namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerModel_VolumeCollision : GameProjectileTriggerModelBase
    {
        public readonly GameEntitySelector checkSelector;

        public GameProjectileTriggerModel_VolumeCollision(
            int actionId,
            GameProjectileStateType nextStateType,
            GameEntitySelector checkSelector
        ) : base(actionId, nextStateType)
        {
            this.checkSelector = checkSelector;
        }

        public void Clear()
        {
        }
    }
}