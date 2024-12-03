using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateTriggerModel_ImpactTarget : GameProjectileStateTriggerModelBase
    {
        public readonly bool checkByTargetCollider;

        public GameProjectileStateTriggerModel_ImpactTarget(
            int actionId,
            GameProjectileStateType nextStateType,
            bool checkByTargetCollider
        ) : base(actionId, nextStateType)
        {
            this.checkByTargetCollider = checkByTargetCollider;
        }

        public void Clear()
        {
        }
    }
}