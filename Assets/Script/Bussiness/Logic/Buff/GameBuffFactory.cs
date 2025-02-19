using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public class GameBuffFactory
    {
        public GameBuffTemplate template { get; private set; }
        public GameBuffFactory()
        {
            template = new GameBuffTemplate();
        }
        public GameBuffEntity Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameBuffFactory.Load: typeId not found: " + typeId);
                return null;
            }
            var entity = new GameBuffEntity(model);
            return entity;
        }
    }
}