using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// buff条件模型 - 当执行行为时
    /// <para>充分必要条件如下---------------------</para>
    /// <para># 行为次数</para>
    /// <para># 有效窗口时间(0表示无限制)</para>
    /// <para>充分不必要条件如下---------------------</para>
    /// <para># 执行某id的行为</para>
    /// <para># 执行某类型的行为</para>
    /// </summary>
    public class GameBuffConditionModel_WhenDoAction
    {
        public readonly int targetActionId;
        public readonly GameActionType targetActionType = GameActionType.None;
        public readonly int actionCount;
        public readonly float validWindowTime;

        public GameBuffConditionModel_WhenDoAction(
            int targetActionId,
            GameActionType targetActionType,
            int actionCount,
            float validWindowTime
        )
        {
            this.targetActionId = targetActionId;
            this.targetActionType = targetActionType;

            this.actionCount = actionCount;
            if (actionCount < 1)
            {
                GameLogger.DebugLog("行为次数必须大于0, 请检查");
                this.actionCount = 1;
            }

            this.validWindowTime = validWindowTime;
        }
    }
}