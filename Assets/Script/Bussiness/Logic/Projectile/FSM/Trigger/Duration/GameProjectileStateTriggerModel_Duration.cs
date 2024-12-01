namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerModel_Duration : GameProjectileStateTriggerModelBase
    {
        public readonly float duration;

        public GameProjectileStateTriggerModel_Duration(int actionId, GameProjectileStateType nextStateType, float duration) : base(actionId, nextStateType)
        {
            this.duration = duration;
        }
    }
}