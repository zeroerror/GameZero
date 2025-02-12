using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Render
{
    public class GameEntityCollectDomainR : GameEntityCollectDomainApiR
    {
        GameContextR _context;

        public GameEntityCollectDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick(float dt)
        {
            {
                var repo = this._context.roleContext.repo;
                repo.ForeachEntities((entity) =>
                {
                    if (entity.isValid) return;
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
                    this._context.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        repo.Recycle(entity);
                    });
                });
            }
            // .......
        }

        public void CollectEntity(GameEntityBase entity)
        {
            if (entity == null) return;
            if (entity is GameRoleEntityR roleEntity)
            {
                this._context.roleContext.repo.Recycle(roleEntity);
                return;
            }
            if (entity is GameSkillEntityR skillEntity)
            {
                this._context.skillContext.repo.Recycle(skillEntity);
                return;
            }
            if (entity is GameProjectileEntityR projectileEntity)
            {
                this._context.projectileContext.repo.Recycle(projectileEntity);
                return;
            }
            if (entity is GameBuffEntityR buffEntity)
            {
                this._context.buffContext.repo.Recycle(buffEntity);
                return;
            }
            GameLogger.LogError($"GameEntityCollectDomainR.CollectEntity: 未知实体类型 {entity.GetType()}");
        }

    }
}