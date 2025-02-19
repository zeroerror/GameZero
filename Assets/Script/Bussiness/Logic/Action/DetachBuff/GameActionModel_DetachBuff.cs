using GamePlay.Core;
using GamePlay.Infrastructure;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_DetachBuff : GameActionModelBase
    {
        /// <summary> buff类型Id </summary>
        public readonly int buffId;
        /// <summary> 层数 </summary>
        public readonly int layer;

        public GameActionModel_DetachBuff(
            int typeId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            in GameVec2 randomValueOffset,
            int buffId,
            int layer
        ) : base(GameActionType.DetachBuff, typeId, selector, preconditionSet, randomValueOffset)
        {

            this.buffId = buffId;
            this.layer = layer;
        }

        public override string ToString()
        {
            return $"buff类型Id:{buffId}, 层数:{layer}";
        }

        public override GameActionModelBase GetCustomModel(float customParam)
        {
            var layer = GameMath.Floor(customParam * this.layer);
            return new GameActionModel_DetachBuff(
                typeId,
                selector,
                preconditionSet,
                randomValueOffset,
                buffId,
                layer
            );
        }
    }
}