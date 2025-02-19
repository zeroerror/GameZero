using GamePlay.Bussiness.Logic;
using GamePlay.Infrastructure;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_AttachBuff
    {
        public GameBuffSO buffSO;
        public int layer;

        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;
        public GameVec2 randomValueOffset;

        public GameActionModel_AttachBuff ToModel()
        {
            if (buffSO == null)
            {
                GameLogger.LogError("GameActionEM_AttachBuff: ToModel: buffSO is null");
                return null;
            }
            var model = new GameActionModel_AttachBuff(
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