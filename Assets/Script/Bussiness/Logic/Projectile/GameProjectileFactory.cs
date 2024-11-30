using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileFactory
    {
        public GameProjectileTemplate template { get; private set; }
        public GameProjectileFactory()
        {
            template = new GameProjectileTemplate();
        }
        public GameProjectileEntity Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameProjectileFactory.Load: typeId not found: " + typeId);
                return null;
            }
            var entity = new GameProjectileEntity(model);
            return entity;
        }
    }
}