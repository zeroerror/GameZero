namespace GamePlay.Bussiness.Logic
{
    public class GameSkillModel
    {
        public readonly int typeId;
        public readonly GameSkillType skillType;
        public readonly string animName;
        public readonly float length;
        public readonly int frameLength;
        public readonly GameTimelineEventModel[] timelineEvModels;

        public readonly GameSkillConditionModel conditionModel;

        public GameSkillModel(
            int typeId,
            GameSkillType skillType,
            string animName,
            float length,
            GameTimelineEventModel[] timelineModels,
            GameSkillConditionModel conditionModel
        )
        {
            this.typeId = typeId;
            this.skillType = skillType;
            this.animName = animName;
            this.length = length;
            this.frameLength = (int)(length * GameTimeCollection.frameRate);
            this.timelineEvModels = timelineModels;
            this.conditionModel = conditionModel;
        }
    }
}