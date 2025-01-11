using GamePlay.Bussiness.Logic;
using GamePlay.Core;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleRepoR : GameEntityRepoBase<GameRoleEntityR>
    {
        /// <summary> 替换旧角色为新角色 </summary>
        public GameRoleEntityR Replace(GameRoleEntityR newRole)
        {
            if (!_dict.TryGetValue(newRole.idCom.entityId, out var oldRole))
            {
                GameLogger.LogError("角色替换失败, 旧角色不存在：" + newRole.idCom.entityId);
                return null;
            }
            _dict[newRole.idCom.entityId] = newRole;
            return oldRole;
        }
    }
}