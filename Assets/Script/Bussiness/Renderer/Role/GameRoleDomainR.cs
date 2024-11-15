using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleDomainR
    {
        GameRoleContextR _context;

        public GameRoleDomainR()
        {
            this._context = new GameRoleContextR();
        }

        public void Collect()
        {
        }

        public GameRoleEntityR Create()
        {
            var e = this._context.factory.Load();
            this._context.repo.TryAdd(e);
            return e;
        }

        public void Tick(float dt)
        {
            var repo = this._context.repo;
            repo.ForeachEntities((entity) =>
            {
                entity.Tick(dt);
            });
        }

        public void PlayAnim(GameRoleEntityR role, string animName)
        {
            var typeId = role.idCom.typeId;
            var url = $"Role/{typeId}/Anim/{animName}";
            var clip = Resources.Load<AnimationClip>(url);
            if (!clip)
            {
                Debug.LogError($"Clip not found: {url}");
                return;
            }
            role.animation.AddClip(clip, animName);
        }
    }

}
