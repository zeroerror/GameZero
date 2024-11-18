namespace GamePlay.Bussiness.Logic
{
    public class GameRoleInputDomain
    {
        GameContext _context;
        GameRoleContext _roleContext => this._context.roleContext;

        public GameRoleInputDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Dispose()
        {
        }

        public void Tick()
        {
            this._roleContext.ForeachPlayerInputArgs((entityId, inputArgs) =>
            {
                var entity = this._roleContext.entityRepo.FindByEntityId(entityId);
                if (entity == null) return;
                entity.inputCom.SetByArgs(inputArgs);
            });
            this._roleContext.ClearPlayerInputArgs();
        }
    }
}