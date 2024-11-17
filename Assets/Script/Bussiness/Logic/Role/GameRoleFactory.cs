using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleFactory
    {
        public GameRoleFactory() { }

        public GameRoleEntity Load(int typeId)
        {
            var e = new GameRoleEntity(typeId);
            GameLogger.Log($"角色工厂: 创建角色实体 {e.idCom.entityId}");
            return e;
        }
    }
}