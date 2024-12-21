using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileModel
    {
        public readonly int typeId;
        public readonly float animLength;
        public readonly GameTimelineEventModel[] timelineEvModels;
        public readonly Dictionary<GameProjectileStateType, GameProjectileTriggerModelSet> triggerSetDict;
        public readonly Dictionary<GameProjectileStateType, object> stateModelDict;
        public readonly float lifeTime;
        public readonly bool isLockRotation;

        public GameProjectileModel(
            int typeId,
            float animLength,
            GameTimelineEventModel[] timelineEvModels,
            Dictionary<GameProjectileStateType, GameProjectileTriggerModelSet> triggerDict,
            Dictionary<GameProjectileStateType, object> stateModelDict,
            float lifeTime,
            bool isLockRotation
        )
        {
            this.typeId = typeId;
            this.animLength = animLength;
            this.timelineEvModels = timelineEvModels;
            this.triggerSetDict = triggerDict;
            this.stateModelDict = stateModelDict;
            this.lifeTime = lifeTime;
            this.isLockRotation = isLockRotation;
        }

    }
}