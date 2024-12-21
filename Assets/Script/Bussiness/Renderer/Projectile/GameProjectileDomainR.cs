using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileDomainR : GameProjectileDomainApiR
    {
        GameContextR _context;
        GameProjectileContextR _projectileContext => this._context.projectileContext;

        public GameProjectileFSMDomain fsmDomain { get; private set; }
        public GameProjectileFSMDomainApiR fsmApi => this.fsmDomain;

        public GameProjectileDomainR()
        {
            this.fsmDomain = new GameProjectileFSMDomain();
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            this.fsmDomain.Inject(context);
            this._BindEvent();
        }

        public void Destroy()
        {
            this.fsmDomain.Destroy();
            this._UnbindEvents();
            this._projectileContext.repo.ForeachAllEntities((entity) =>
            {
                entity.Destroy();
            });
        }

        public void Tick(float dt)
        {
            this._projectileContext.repo.ForeachEntities((e) =>
            {
                e.Tick(dt);
                this.fsmDomain.Tick(e, dt);
            });
        }

        private void _BindEvent()
        {
            this._context.BindRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_CREATE, this._OnProjectileCreate);
        }

        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_CREATE, this._OnProjectileCreate);
        }

        private void _OnProjectileCreate(object args)
        {
            var evArgs = (GameProjectileRCArgs_Create)args;
            var creator = this._context.FindEntity(evArgs.creatorIdArgs);
            if (creator == null)
            {
                this._context.DelayRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_CREATE, args);
                return;
            }

            this.CreateProjectile(evArgs.idArgs, creator, evArgs.transArgs);
        }

        private void CreateProjectile(in GameIdArgs idArgs, GameEntityBase creator, in GameTransformArgs transArgs)
        {
            var repo = this._projectileContext.repo;
            var typeId = idArgs.typeId;
            if (!repo.TryFetch(typeId, out var projectile))
            {
                projectile = this._projectileContext.factory.Load(typeId);
                if (projectile == null)
                {
                    GameLogger.LogError("[R]弹道创建失败，弹道ID不存在：" + typeId);
                    return;
                }
            }

            projectile.idCom.SetEntityId(this._projectileContext.idService.FetchId());
            projectile.idCom.SetParent(creator);
            projectile.transformCom.SetByArgs(transArgs);
            projectile.SyncTrans();
            if (projectile.model.animClip) projectile.animCom.Play(projectile.model.animClip);
            repo.TryAdd(projectile);
            projectile.setActive(true);

            var orderOffset = projectile.model.isLockRotation ? 0 : 1000;
            this._context.domainApi.fielApi.AddToLayer(projectile.root, GameFieldLayerType.Entity, orderOffset);
        }

    }
}