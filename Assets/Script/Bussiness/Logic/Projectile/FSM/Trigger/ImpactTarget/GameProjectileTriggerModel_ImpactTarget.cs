namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerModel_ImpactTarget : GameProjectileTriggerModelBase
    {
        public readonly GameEntitySelector detectEntitySelector;
        public readonly bool detectByTargetCollider;

        public GameProjectileTriggerModel_ImpactTarget(
            int[] actionIds,
            GameProjectileStateType nextStateType,
            GameEntitySelector selector,
            bool checkByTargetCollider
        ) : base(actionIds, nextStateType)
        {
            this.detectEntitySelector = selector;
            this.detectByTargetCollider = checkByTargetCollider;
        }

        public void Clear()
        {
        }
    }
}