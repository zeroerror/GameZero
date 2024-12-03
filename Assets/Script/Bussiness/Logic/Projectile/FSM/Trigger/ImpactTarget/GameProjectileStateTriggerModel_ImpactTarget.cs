using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerModel_ImpactTarget : GameProjectileStateTriggerModelBase
    {
        public GameProjectileStateTriggerModel_ImpactTarget(
            int actionId,
            GameProjectileStateType nextStateType
        ) : base(actionId, nextStateType)
        {
        }

        public void Clear()
        {
        }
    }
}