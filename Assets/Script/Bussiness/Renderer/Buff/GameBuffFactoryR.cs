using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameBuffFactoryR
    {
        public GameBuffTemplateR template { get; private set; }
        public GameBuffFactoryR()
        {
            template = new GameBuffTemplateR();
        }
        public GameBuffEntityR Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameBuffFactoryR.Load: typeId not found: " + typeId);
                return null;
            }
            var entity = new GameBuffEntityR(model);
            return entity;
        }
    }
}