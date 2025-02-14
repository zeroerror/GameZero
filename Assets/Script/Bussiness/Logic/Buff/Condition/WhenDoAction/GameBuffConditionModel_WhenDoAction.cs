using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// buff条件模型 - 当执行行为时
    /// </summary>
    public class GameBuffConditionModel_WhenDoAction
    {
        // --------------------- 充分不必要条件 ---------------------
        /// <summary> 目标行为Id </summary>
        public readonly int targetActionId;
        /// <summary> 目标行为类型 </summary>
        public readonly GameActionType targetActionType = GameActionType.None;

        // --------------------- 充分必要条件 ---------------------
        /// <summary> 行为次数 </summary>
        public readonly int actionCount;
        /// <summary> 有效窗口时间 </summary>
        public readonly float validWindowTime;
        /// <summary> buff持有者是否作为行为目标 </summary>
        public readonly bool isBuffOwnerAsTarget;

        public GameBuffConditionModel_WhenDoAction(
            int targetActionId,
            GameActionType targetActionType,
            int actionCount,
            float validWindowTime,
            bool isBuffOwnerAsTarget)
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
            this.isBuffOwnerAsTarget = isBuffOwnerAsTarget;
        }
    }
}