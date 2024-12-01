namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerModel_VolumeCollision
    {
        public readonly int actionId;
        public readonly GameProjectileStateType nextStateType;

        public GameProjectileStateTriggerModel_VolumeCollision(int actionId, GameProjectileStateType nextStateType)
        {
            this.actionId = actionId;
            this.nextStateType = nextStateType;
        }

        public void Clear()
        {
        }
    }
}