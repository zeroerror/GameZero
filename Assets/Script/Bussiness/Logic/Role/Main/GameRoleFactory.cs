using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleFactory
    {
        public GameRoleTemplate template { get; private set; }

        public GameRoleFactory()
        {
            template = new GameRoleTemplate();
        }

        public GameRoleEntity Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("角色创建失败，角色ID不存在：" + typeId);
                return null;
            }
            var e = new GameRoleEntity(model);
            return e;
        }
    }
}