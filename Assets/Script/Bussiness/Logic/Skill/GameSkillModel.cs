namespace GamePlay.Bussiness.Logic
{
    public enum GameSkillTargterType
    {
        None,
        /// <summary>
        /// 选中的敌人
        /// </summary>
        Enemy,
        /// <summary>
        /// 选中的方向
        /// </summary>
        Direction,
        /// <summary>
        /// 选中的位置
        /// </summary>
        Position
    }
    public class GameSkillModel
    {
        public readonly int typeId;
        public readonly string animName;
        public readonly float length;
        public readonly int frameLength;
        public readonly GameTimelineEventModel[] timelineEvModels;

        public readonly GameSkillConditionModel conditionModel;

        /// <summary>
        /// typeId: 技能类型ID
        /// animName: 动作名称
        /// length: 动作时长
        /// timelineModels: 时间轴事件模型
        /// </summary>
        public GameSkillModel(int typeId, string animName, float length, GameTimelineEventModel[] timelineModels, GameSkillConditionModel conditionModel)
        {
            this.typeId = typeId;
            this.animName = animName;
            this.length = length;
            this.frameLength = (int)(length * GameTimeCollection.frameRate);
            this.timelineEvModels = timelineModels;
            this.conditionModel = conditionModel;
        }
    }
}