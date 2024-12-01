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
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            this._BindEvent();
        }

        public void Dispose()
        {
            this._UnbindEvents();
        }

        private void _BindEvent()
        {
        }

        private void _UnbindEvents()
        {
        }

        public void Tick(float dt)
        {
        }

        public GameProjectileEntity CreateProjectile(int typeId, GameEntityBase creator, in GameTransformArgs transArgs)
        {
            var repo = this._projectileContext.repo;
            if (!repo.TryFetch(typeId, out var e)) e = this._projectileContext.factory.Load(typeId);
            if (e == null)
            {
                GameLogger.LogError("弹道创建失败，弹道ID不存在：" + typeId);
                return null;
            }
            e.transformCom.SetByArgs(transArgs);
            e.idCom.entityId = this._projectileContext.idService.FetchId();
            e.idCom.SetParent(creator);
            var colliderModel = new GameBoxColliderModel(new GameVec2(0, 0), 0, 0.5f, 0.5f);
            this._context.domainApi.physicsApi.CreatePhysics(e, colliderModel);
            repo.TryAdd(e);

            // 提交RC
            var args = new GameProjectileRCArgs_Create()
            {
                idArgs = e.idCom.ToArgs(),
                transArgs = e.transformCom.ToArgs(),
            };
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_CREATE, args);
            return e;
        }
    }
}