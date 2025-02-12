using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleRepo : GameEntityRepoBase<GameRoleEntity>
    {
        /// <summary> 替换旧角色为新角色 </summary>
        public GameRoleEntity Replace(GameRoleEntity newRole)
        {
            if (!this._dict.TryGetValue(newRole.idCom.entityId, out var oldRole))
            {
                GameLogger.LogError("角色替换失败, 旧角色不存在：" + newRole.idCom.entityId);
                return null;
            }
            this._dict[newRole.idCom.entityId] = newRole;
            this._list[this._list.IndexOf(oldRole)] = newRole;
            return oldRole;
        }

        public GameRoleEntity GetNearestEnemy(GameRoleEntity role)
        {
            if (!role) return null;
            var nearestDis = float.MaxValue;
            GameRoleEntity nearestRole = null;
            var pos = role.transformCom.position;
            this._list.Foreach((enemy) =>
            {
                if (enemy == role) return;
                if (!enemy.idCom.CheckCampType(role.idCom, GameCampType.Enemy)) return;
                var dis = (pos - enemy.transformCom.position).sqrMagnitude;
                if (dis < nearestDis)
                {
                    nearestDis = dis;
                    nearestRole = enemy;
                }
            });
            return nearestRole;
        }
    }
}