using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileModel
    {
        public readonly int typeId;
        public readonly float animLength;
        public readonly GameTimelineEventModel[] timelineEvModels;
        public readonly Dictionary<GameProjectileStateType, GameProjectileStateTriggerModelSet> stateTriggerSetDict;

        public GameProjectileModel(int typeId, float animLength, GameTimelineEventModel[] timelineEvModels, Dictionary<GameProjectileStateType, GameProjectileStateTriggerModelSet> stateTriggerDict)
        {
            this.typeId = typeId;
            this.animLength = animLength;
            this.timelineEvModels = timelineEvModels;
            this.stateTriggerSetDict = stateTriggerDict;
        }

    }
}