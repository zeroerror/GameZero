using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleFactory
    {
        public GameRoleEntity Load()
        {
            var e = new GameRoleEntity();
            GameLogger.Log($"角色工厂: 创建角色实体 {e.idCom.entityId}");
            return e;
        }
    }
}