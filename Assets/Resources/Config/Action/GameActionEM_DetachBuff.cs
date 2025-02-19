using GameVec2 = UnityEngine.Vector2;
using GamePlay.Bussiness.Logic;
using GamePlay.Infrastructure;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_DetachBuff
    {
        public GameBuffSO buffSO;
        public int layer;

        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;
        public GameVec2 randomValueOffset;

        public GameActionModel_DetachBuff ToModel()
        {
            if (buffSO == null)
            {
                GameLogger.LogError("GameActionEM_DetachBuff: ToModel: buffSO is null");
                return null;
            }
            var model = new GameActionModel_DetachBuff(
                0,
                this.selectorEM.ToModel(),
                this.preconditionSetEM?.ToModel(),
                this.randomValueOffset,
                this.buffSO.typeId,
                this.layer
            );
            return model;
        }
    }
}