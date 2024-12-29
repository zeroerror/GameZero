namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// buff条件模型 - 当执行行为时
    /// <para>当满足以下任意条件时，都算作满足条件</para>
    /// <para># 执行某id的行为</para>
    /// <para># 执行某类型的行为</para>
    /// </summary>
    public class GameBuffConditionModel_WhenDoAction
    {
        public readonly int targetActionId;
        public readonly GameActionType targetActionType = GameActionType.None;

        public GameBuffConditionModel_WhenDoAction(int targetActionId, GameActionType targetActionType)
        {
            this.targetActionId = targetActionId;
            this.targetActionType = targetActionType;
        }
    }
}