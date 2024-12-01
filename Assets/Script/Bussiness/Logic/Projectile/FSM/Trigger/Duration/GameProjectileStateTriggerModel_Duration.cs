namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerModel_Duration
    {
        public readonly int actionId;
        public readonly GameProjectileStateType nextStateType;
        public readonly float duration;

        public float elapsedTime;
        public bool isSatisfied;

        public GameProjectileStateTriggerModel_Duration(int actionId, GameProjectileStateType nextStateType, float duration)
        {
            this.actionId = actionId;
            this.nextStateType = nextStateType;
            this.duration = duration;
        }

        public void Clear()
        {
            this.elapsedTime = 0;
            this.isSatisfied = false;
        }
    }
}