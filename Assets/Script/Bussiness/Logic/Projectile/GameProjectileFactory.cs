using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileFactoryR
    {
        public GameProjectileTemplateR template { get; private set; }
        public GameProjectileFactoryR()
        {
            template = new GameProjectileTemplateR();
        }
        public GameProjectileEntity Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("投射物创建失败，投射物ID不存在：" + typeId);
                return null;
            }
            var entity = new GameProjectileEntity(model);
            return entity;
        }
    }
}