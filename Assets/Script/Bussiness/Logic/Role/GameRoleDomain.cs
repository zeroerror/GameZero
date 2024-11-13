using Unity.VisualScripting;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleDomain
    {
        GameRoleContext _context;

        public GameRoleDomain()
        {
            this._context = new GameRoleContext();
        }

        public void Collect()
        {
        }

        public GameRoleEntity Create()
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
    }
}
