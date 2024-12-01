namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileModel
    {
        public readonly int typeId;
        public readonly float animLength;
        public readonly GameTimelineEventModel[] timelineEvModels;
        public readonly int collisionActionId;

        public GameProjectileModel(int typeId, GameTimelineEventModel[] timelineEvModels, float animLength, int collisionActionId)
        {
            this.typeId = typeId;
            this.timelineEvModels = timelineEvModels;
            this.animLength = animLength;
            this.collisionActionId = collisionActionId;
        }
    }
}