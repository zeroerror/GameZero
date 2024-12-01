using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileFactoryR
    {
        public GameProjectileTemplateR template { get; private set; }
        public GameProjectileFactoryR()
        {
            template = new GameProjectileTemplateR();
        }
        public GameProjectileEntityR Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameProjectileFactoryR.Load: typeId not found: " + typeId);
                return null;
            }
            var entity = new GameProjectileEntityR(model);
            return entity;
        }
    }
}