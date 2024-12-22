namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_KnockBack : GameActionModelBase
    {
        /// <summary> 击退方向类型 </summary>
        public readonly GameActionKnockBackDirType knockBackDirType;

        /// <summary> 距离 </summary>
        public readonly float distance;
        /// <summary> 持续时间 </summary>
        public readonly float duration;
        /// <summary> 缓动曲线 </summary>
        public readonly GameEasingType easingType;

        public GameActionModel_KnockBack(
            int typeId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            GameActionKnockBackDirType knockBackDirType,
            float distance,
            float duration,
            GameEasingType easingType
        ) : base(GameActionType.KnockBack, typeId, selector, preconditionSet)
        {
            this.knockBackDirType = knockBackDirType;
            this.distance = distance;
            this.duration = duration;
            this.easingType = easingType;
        }
    }
}