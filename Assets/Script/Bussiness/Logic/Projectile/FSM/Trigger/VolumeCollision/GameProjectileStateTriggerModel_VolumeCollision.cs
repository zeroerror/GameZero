namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerModel_VolumeCollision : GameProjectileStateTriggerModelBase
    {
        public readonly GameEntitySelector checkSelector;

        public GameProjectileStateTriggerModel_VolumeCollision(
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