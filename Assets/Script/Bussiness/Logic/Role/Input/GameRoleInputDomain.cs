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
            this._roleContext.entityRepo.ForeachEntities((entity) =>
            {
                entity.inputCom.Clear();
                if (this._roleContext.TryGetPlayerInputArgs(entity.idCom.entityId, out var inputArgs))
                {
                    entity.inputCom.SetByArgs(inputArgs);
                }
            });
            this._roleContext.ClearPlayerInputArgs();
        }
    }
}