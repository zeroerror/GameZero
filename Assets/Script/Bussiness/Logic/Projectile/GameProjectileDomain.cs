using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileDomain : GameProjectileDomainApi
    {
        GameContext _context;
        GameProjectileContext _projectileContext => this._context.projectileContext;

        public GameProjectileFSMDomain fsmDomain { get; private set; }
        public GameProjectileFSMDomainApi fsmApi => this.fsmDomain;

        public GameProjectileDomain()
        {
            this.fsmDomain = new GameProjectileFSMDomain();
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            this.fsmDomain.Inject(context);
        }

        public void Dispose()
        {
            this.fsmDomain.Dispose();
        }

        public void Tick(float dt)
        {
            var repo = this._projectileContext.repo;
            repo.ForeachEntities((e) =>
            {
                e.Tick(dt);
                this.fsmDomain.Tick(e, dt);
            });
        }

        public GameProjectileEntity CreateProjectile(int typeId, GameEntityBase creator, GameTransformArgs transArgs, in GameActionTargeterArgs targeter)
        {
            var repo = this._projectileContext.repo;
            if (!repo.TryFetch(typeId, out var projectile)) projectile = this._projectileContext.factory.Load(typeId);
            if (projectile == null)
            {
                GameLogger.LogError("弹道创建失败，弹道ID不存在：" + typeId);
                return null;
            }
            // 保持缩放为正
            var scale = transArgs.scale;
            scale.x = GameMathF.Abs(scale.x);
            scale.y = GameMathF.Abs(scale.y);
            transArgs.scale = scale;

            projectile.transformCom.SetByArgs(transArgs);
            projectile.idCom.entityId = this._projectileContext.idService.FetchId();
            projectile.idCom.SetParent(creator);
            projectile.actionTargeterCom.SetTargeter(targeter);
            repo.TryAdd(projectile);

            // 提交RC
            var args = new GameProjectileRCArgs_Create()
            {
                idArgs = projectile.idCom.ToArgs(),
                creatorIdArgs = creator.idCom.ToArgs(),
                transArgs = projectile.transformCom.ToArgs(),
            };
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_CREATE, args);

            // 默认进入待机
            this.fsmDomain.InitFSM(projectile);

            return projectile;
        }
    }
}