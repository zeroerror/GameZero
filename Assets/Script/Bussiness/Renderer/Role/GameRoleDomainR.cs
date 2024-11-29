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

        public void Inject(GameContextR context)
        {
            this._context = context;
            this.inputDomain.Inject(context);
            this.fsmDomain.Inject(context);
            this._BindEvent();
        }

        public void Dispose()
        {
            this._UnbindEvents();
            this.fsmDomain.Dispose();
            this._roleContext.repo.ForeachEntities((entity) =>
            {
                entity.Dispose();
            });
        }

        private void _BindEvent()
        {
            this._context.BindRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, this._OnRoleCreate);
            this.fsmDomain.BindEvent();
        }

        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, this._OnRoleCreate);
            this.fsmDomain.UnbindEvents();
        }

        private void _OnRoleCreate(object args)
        {
            var evArgs = (GameRoleRCArgs_Create)args;
            this._Create(evArgs.idArgs, evArgs.transComArgs, evArgs.isUser);
        }

        public void Collect()
        {
        }

        private GameRoleEntityR _Create(in GameIdArgs idArgs, in GameTransformArgs transArgs, bool isUser = false)
        {
            var role = this._roleContext.factory.Load(idArgs.typeId);
            role.idCom.SetByArgs(idArgs);
            role.transformCom.SetByArgs(transArgs);
            role.SyncTrans();
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

        public void PlayAnim(GameRoleEntityR entity, string animName)
        {
            var factory = this._roleContext.factory;
            var animCom = entity.animCom;
            if (animCom.hasClip(animName))
            {
                animCom.Play(animName);
            }
            else
            {
                var clip = factory.LoadAnimationClip(entity.idCom.typeId, animName);
                animCom.Play(clip);
            }
        }

    }

}
