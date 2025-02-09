using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public interface GameRoleDomainApiR
    {
        public void PlayAnim(GameRoleEntityR entity, string animName);
        public GameRoleTemplateR GetRoleTemplate();

        /// <summary>
        /// 根据实体Id查找角色
        /// <para>entityId: 实体Id</para>
        /// </summary>
        public GameRoleEntityR FindByEntityId(int entityId);

    }

}
