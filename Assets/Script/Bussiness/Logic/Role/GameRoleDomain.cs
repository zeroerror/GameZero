using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleDomain : GameRoleDomainApi
    {
        GameContext _context;
        GameRoleContext _roleContext => this._context.roleContext;

        public GameRoleInputDomain inputDomain { get; private set; }
        public GameRoleFSMDomain fsmDomain { get; private set; }
        public GameRoleFSMDomainApi fsmApi => this.fsmDomain;

        public GameRoleDomain()
        {
            this.inputDomain = new GameRoleInputDomain();
            this.fsmDomain = new GameRoleFSMDomain();
        }

        public void Inject(GameContext context)
        {
            this._context = context;
            this.inputDomain.Inject(context);
            this.fsmDomain.Inject(context);
        }

        public void Dispose()
        {
            this.inputDomain.Dispose();
            this.fsmDomain.Dispose();
        }

        public void Collect(GameRoleEntity role)
        {
            this._context.domainApi.physicsApi.RemovePhysics(role);
        }

        public GameRoleEntity CreatePlayerRole(int typeId, int campId, in GameTransformArgs transArgs, bool isUser)
        {
            var e = this.CreateRole(typeId, campId, transArgs, isUser);
            if (this._roleContext.userRole == null) this._roleContext.userRole = e;
            return e;
        }

        public GameRoleEntity CreateRole(int typeId, int campId, in GameTransformArgs transArgs, bool isUser)
        {
            var repo = this._roleContext.repo;
            if (!repo.TryFetch(typeId, out var e)) e = this._roleContext.factory.Load(typeId);
            if (e == null)
            {
                GameLogger.LogError("角色创建失败，角色ID不存在：" + typeId);
                return null;
            }
            e.transformCom.SetByArgs(transArgs);
            e.idCom.SetEntityId(this._roleContext.idService.FetchId());
            e.idCom.campId = campId;
            var colliderModel = new GameBoxColliderModel(new GameVec2(0, 0.625f), 0, 0.7f, 1.25f);
            this._context.domainApi.physicsApi.CreatePhysics(e, colliderModel);
            this._context.domainApi.skillApi.CreateSkill(e, 1001);
            this._context.domainApi.skillApi.CreateSkill(e, 1002);
            this._context.domainApi.skillApi.CreateSkill(e, 1003);
            repo.TryAdd(e);

            // 提交RC事件
            this._context.SubmitRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, new GameRoleRCArgs_Create
            {
                idArgs = e.idCom.ToArgs(),
                transArgs = e.transformCom.ToArgs(),
                isUser = isUser
            });

            // 默认进入待机
            this.fsmDomain.TryEnter(e, GameRoleStateType.Idle);

            return e;
        }

        public void Tick(float dt)
        {
            this.inputDomain.Tick();
            var repo = this._roleContext.repo;
            repo.ForeachEntities((entity) =>
            {
                entity.Tick(dt);
                this.fsmDomain.Tick(entity, dt);
            });
        }

        public GameRoleEntity GetNearestEnemy(GameEntityBase entity)
        {
            var nearestEnemy = default(GameRoleEntity);
            var nearestDistance = float.MaxValue;
            this._roleContext.repo.ForeachEntities((e) =>
            {
                if (e == entity) return;
                if (e.idCom.campId == entity.idCom.campId) return;
                var disSqr = (e.transformCom.position - entity.transformCom.position).sqrMagnitude;
                if (disSqr < nearestDistance)
                {
                    nearestDistance = disSqr;
                    nearestEnemy = e;
                }
            });
            return nearestEnemy;
        }
    }
}
