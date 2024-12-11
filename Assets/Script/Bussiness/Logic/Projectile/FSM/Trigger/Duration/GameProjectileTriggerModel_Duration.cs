namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerModel_Duration : GameProjectileTriggerModelBase
    {
        public readonly float duration;

        public GameProjectileTriggerModel_Duration(int[] actionIds, GameProjectileStateType nextStateType, float duration) : base(actionIds, nextStateType)
        {
            this.duration = duration;
        }
    }
}