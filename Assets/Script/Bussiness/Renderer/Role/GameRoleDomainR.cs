using GamePlay.Bussiness.Logic;
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
    }
}
