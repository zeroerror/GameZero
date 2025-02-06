using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public interface GameRoleDomainApiR
    {
        public void PlayAnim(GameRoleEntityR entity, string animName);
        public GameRoleTemplateR GetRoleTemplate();
    }

}
