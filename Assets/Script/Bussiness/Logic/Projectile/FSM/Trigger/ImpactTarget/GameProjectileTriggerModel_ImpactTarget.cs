using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileTriggerModel_ImpactTarget : GameProjectileTriggerModelBase
    {
        public readonly GameEntitySelector checkSelector;
        public readonly bool checkByTargetCollider;

        public GameProjectileTriggerModel_ImpactTarget(
            int actionId,
            GameProjectileStateType nextStateType,
            GameEntitySelector selector,
            bool checkByTargetCollider
        ) : base(actionId, nextStateType)
        {
            this.checkSelector = selector;
            this.checkByTargetCollider = checkByTargetCollider;
        }

        public void Clear()
        {
        }
    }
}