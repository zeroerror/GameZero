namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerModel_Duration : GameProjectileTriggerModelBase
    {
        public readonly float duration;

        public GameProjectileTriggerModel_Duration(int actionId, GameProjectileStateType nextStateType, float duration) : base(actionId, nextStateType)
        {
            this.duration = duration;
        }
    }
}