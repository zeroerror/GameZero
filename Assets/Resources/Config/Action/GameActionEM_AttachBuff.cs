using GamePlay.Bussiness.Logic;
using GamePlay.Core;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEM_AttachBuff
    {
        public GameBuffSO buffSO;
        public int layer;

        public GameEntitySelectorEM selectorEM;
        public GameActionPreconditionSetEM preconditionSetEM;

        public GameActionModel_AttachBuff ToModel()
        {
            if (buffSO == null)
            {
                GameLogger.LogError("GameActionEM_AttachBuff: ToModel: buffSO is null");
                return null;
            }
            var model = new GameActionModel_AttachBuff(
                0,
                selectorEM.ToSelector(),
                preconditionSetEM?.ToModel(),
                buffSO.typeId,
                layer
            );
            return model;
        }
    }
}