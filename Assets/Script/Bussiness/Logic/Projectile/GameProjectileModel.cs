namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileModel
    {
        public readonly int typeId;
        public readonly GameTimelineEventModel[] timelineEvModels;

        public GameProjectileModel(int typeId, GameTimelineEventModel[] timelineModels)
        {
            this.typeId = typeId;
            this.timelineEvModels = timelineModels;
        }
    }
}