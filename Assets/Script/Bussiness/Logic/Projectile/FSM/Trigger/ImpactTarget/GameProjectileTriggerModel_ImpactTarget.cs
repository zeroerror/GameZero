namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerModel_ImpactTarget : GameProjectileTriggerModelBase
    {
        public readonly GameEntitySelector detectEntitySelector;
        public readonly bool detectByTargetCollider;

        public GameProjectileTriggerModel_ImpactTarget(
            int actionId,
            GameProjectileStateType nextStateType,
            GameEntitySelector selector,
            bool checkByTargetCollider
        ) : base(actionId, nextStateType)
        {
            this.detectEntitySelector = selector;
            this.detectByTargetCollider = checkByTargetCollider;
        }

        public void Clear()
        {
        }
    }
}