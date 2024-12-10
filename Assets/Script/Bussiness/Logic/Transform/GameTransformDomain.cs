
namespace GamePlay.Bussiness.Logic
{
    public class GameTransformDomain : GameTransformDomainApi
    {
        GameContext _context;

        public GameTransformDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Dispose()
        {
        }

        public void Tick(float dt)
        {
            this._context.roleContext.repo.ForeachEntities(this._RCTransformDirty);
            this._context.projectileContext.repo.ForeachEntities(this._RCTransformDirty);
        }

        private void _RCTransformDirty(GameEntityBase entity)
        {
            if (!entity.transformCom.CheckDirty()) return;
            this._context.SubmitRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC, new GameTransformRCArgs_Sync()
            {
                idArgs = entity.idCom.ToArgs(),
                transArgs = entity.transformCom.ToArgs()
            });
        }
    }
}
