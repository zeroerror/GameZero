using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleDomain : GameRoleDomainApi
    {
        GameContext _context;
        GameRoleContext _roleContext => this._context.roleContext;

        public GameRoleInputDomain inputDomain { get; private set; }
        public GameRoleFSMDomain fsmDomain { get; private set; }

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
            this.CreateUserRole(1000, 1, new GameTransformArgs { position = new GameVec2(0, 0), angle = 0, scale = 1 });
        }

        public void Dispose()
        {
            this.inputDomain.Dispose();
            this.fsmDomain.Dispose();
        }

        public void Collect()
        {
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
            e.idCom.entityId = this._roleContext.entityIdService.FetchEntityId();
            e.idCom.campId = campId;
            this._roleContext.entityRepo.TryAdd(e);

            // 提交RC事件
            this._context.rcEventService.Submit(GameRoleRCCollection.RC_GAME_ROLE_CREATE, new GameRoleRCCollection.GameRoleRCArgs_Create
            {
                idComArgs = e.idCom.ToArgs(),
                transComArgs = e.transformCom.ToArgs(),
                isUser = isUser
            });

            this.fsmDomain.Enter(e, GameRoleStateType.Idle);

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
