namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_AttachBuff : GameActionModelBase
    {
        /// <summary> buff类型Id </summary>
        public readonly int buffId;
        /// <summary> 层数 </summary>
        public readonly int layer;

        public GameActionModel_AttachBuff(
            int typeId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            int buffId,
            int layer
        ) : base(GameActionType.AttachBuff, typeId, selector, preconditionSet)
        {

            this.buffId = buffId;
            this.layer = layer;
        }

        public override string ToString()
        {
            return $"buff类型Id:{buffId}, 层数:{layer}";
        }
    }
}