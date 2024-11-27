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

        public GameRoleEntity CreateUserRole(int typeId, int campId, in GameTransformArgs transArgs)
        {
            var e = this.Create(typeId, campId, transArgs, true);
            this._roleContext.userRole = e;
            return e;
        }

        public GameRoleEntity Create(int typeId, int campId, in GameTransformArgs transArgs, bool isUser = false)
        {
            var e = this._roleContext.factory.Load(typeId);
            e.transformCom.SetByArgs(transArgs);
            e.idCom.entityId = this._roleContext.entityIdService.FetchId();
            e.idCom.campId = campId;
            var colliderModel = new GameBoxColliderModel(new GameVec2(0, 0.625f), 0, 0.7f, 1.25f);
            this._context.domainApi.physicsApi.CreatePhysics(e, colliderModel);
            this._context.domainApi.skillApi.CreateSkill(e, 1001);
            this._context.domainApi.skillApi.CreateSkill(e, 1002);
            this._context.domainApi.skillApi.CreateSkill(e, 1003);
            this._roleContext.entityRepo.TryAdd(e);

            // 提交RC事件
            this._context.rcEventService.Submit(GameRoleRCCollection.RC_GAME_ROLE_CREATE, new GameRoleRCArgs_Create
            {
                idArgs = e.idCom.ToArgs(),
                transComArgs = e.transformCom.ToArgs(),
                isUser = isUser
            });

            this.fsmDomain.TryEnter(e, GameRoleStateType.Idle);

            return e;
        }

        public void Tick(float dt)
        {
            this.inputDomain.Tick();
            var repo = this._roleContext.entityRepo;
            repo.ForeachEntities((entity) =>
            {
                entity.Tick(dt);
                this.fsmDomain.Tick(entity, dt);
            });
        }
    }
}
