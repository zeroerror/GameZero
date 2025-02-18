namespace GamePlay.Bussiness.Logic
{
    public class GameSkillModel
    {
        public readonly int typeId;
        public readonly GameSkillType skillType;
        public readonly float length;
        public readonly int frameLength;

        public readonly GameTimelineEventModel[] timelineEvModels;
        public readonly GameSkillConditionModel conditionModel;
        public readonly GameSkillMovementModel movementModel;

        public GameSkillModel(
            int typeId,
            GameSkillType skillType,
            float length,
            GameTimelineEventModel[] timelineModels,
            GameSkillConditionModel conditionModel,
            GameSkillMovementModel movementModel
        )
        {
            this.typeId = typeId;
            this.skillType = skillType;
            this.length = length;
            this.frameLength = (int)(length * GameTimeCollection.frameRate);
            this.timelineEvModels = timelineModels;
            this.conditionModel = conditionModel;
            this.movementModel = movementModel;
        }

        /// <summary> 是否受到攻速的影响 </summary>
        public bool effectByAttackSpeed
        {
            get
            {
                return this.skillType == GameSkillType.NormalAttack;
            }
        }
    }
}