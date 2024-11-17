using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleDomainR
    {
        GameContextR _context;
        GameRoleContextR _roleContext => this._context.roleContext;
        public GameRoleDomain_FSMR fsmDomain { get; private set; }

        public GameRoleDomainR(GameContextR context)
        {
            this._context = context;
            this.fsmDomain = new GameRoleDomain_FSMR(context);
            this.Create(1000);//test
        }

        public void Dispose()
        {
            this.fsmDomain.Dispose();
            this._roleContext.repo.ForeachEntities((entity) =>
            {
                entity.Dispose();
            });
        }

        public void Collect()
        {
        }

        public GameRoleEntityR Create(int typeId)
        {
            var e = this._roleContext.factory.Load(typeId);
            this._roleContext.repo.TryAdd(e);
            return e;
        }

        public void Tick(float dt)
        {
            var repo = this._roleContext.repo;
            repo.ForeachEntities((entity) =>
            {
                entity.Tick(dt);
                this.fsmDomain.Tick(dt, entity);
            });
        }

    }

}
