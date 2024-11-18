using GamePlay.Bussiness.Logic;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleDomainR : GameRoleDomainApiR
    {
        GameContextR _context;
        GameRoleContextR _roleContext => this._context.roleContext;

        public GameRoleInputDomainR inputDomain { get; private set; }
        public GameRoleFSMDomainR fsmDomain { get; private set; }

        public GameRoleDomainR()
        {
            this.inputDomain = new GameRoleInputDomainR();
            this.fsmDomain = new GameRoleFSMDomainR();
        }

        private void _BindEvents()
        {
            this._context.logicContext.rcEventService.Regist(GameRoleRCCollection.RC_GAME_ROLE_CREATE, this._OnRoleCreate);
        }

        private void _OnRoleCreate(object args)
        {
            var evArgs = (GameRoleRCCollection.GameRoleRCArgs_Create)args;
            this.Create(evArgs.idComArgs, evArgs.transComArgs, evArgs.isUser);
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            this.inputDomain.Inject(context);
            this.fsmDomain.Inject(context);
            this._BindEvents();
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

        public GameRoleEntityR Create(in GameIdArgs idArgs, in GameTransformArgs transArgs, bool isUser = false)
        {
            var role = this._roleContext.factory.Load(idArgs.typeId);
            role.idCom.SetByArgs(idArgs);
            role.transformCom.SetByArgs(transArgs);
            this._roleContext.repo.TryAdd(role);
            if (isUser) this._roleContext.userRole = role;
            return role;
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

    }

}
