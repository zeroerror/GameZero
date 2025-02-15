namespace GamePlay.Bussiness.Logic
{
    public class GameEntityCollectDomain : GameEntityCollectDomainApi
    {
        GameContext _context;

        public GameEntityCollectDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick()
        {
            {
                var repo = this._context.roleContext.repo;
                repo.ForeachEntities((entity) =>
                {
                    if (entity.isValid) return;
                    if (entity.HasReference()) return;
                    this._context.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        repo.Recycle(entity);
                    });
                });
            }
            {
                var repo = this._context.skillContext.repo;
                repo.ForeachEntities((entity) =>
                {
                    if (entity.isValid) return;
                    if (entity.HasReference()) return;
                    this._context.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        repo.Recycle(entity);
                    });
                });
            }
            {
                var repo = this._context.projectileContext.repo;
                repo.ForeachEntities((entity) =>
                {
                    if (entity.isValid) return;
                    if (entity.HasReference()) return;
                    this._context.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        repo.Recycle(entity);
                    });
                });
            }
            {
                var repo = this._context.buffContext.repo;
                repo.ForeachEntities((entity) =>
                {
                    if (entity.isValid) return;
                    if (entity.HasReference()) return;
                    this._context.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        repo.Recycle(entity);
                    });
                });
            }
            // .......
        }
    }
}