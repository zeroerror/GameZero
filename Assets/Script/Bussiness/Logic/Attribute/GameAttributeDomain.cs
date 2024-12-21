
namespace GamePlay.Bussiness.Logic
{
    public class GameAttributeDomain : GameAttributeDomainApi
    {
        GameContext _context;

        public GameAttributeDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick(float dt)
        {
            this._context.roleContext.repo.ForeachEntities(this._RCAttributeDirty);
        }

        private void _RCAttributeDirty(GameEntityBase entity)
        {
            var attributeCom = entity.attributeCom;
            if (attributeCom.CheckDirty())
            {
                attributeCom.ClearDirty();
                this._context.SubmitRC(GameAttributeRCCollection.RC_GAME_ATTRIBUTE_SYNC, new GameAttributeRCArgs_Sync()
                {
                    idArgs = entity.idCom.ToArgs(),
                    attrAgrs = attributeCom.ToArgs()
                });
            }

            var baseAttributeCom = entity.baseAttributeCom;
            if (baseAttributeCom.CheckDirty())
            {
                baseAttributeCom.ClearDirty();
                this._context.SubmitRC(GameAttributeRCCollection.RC_GAME_ATTRIBUTE_BASE_SYNC, new GameAttributeRCArgs_BaseSync()
                {
                    idArgs = entity.idCom.ToArgs(),
                    attrAgrs = baseAttributeCom.ToArgs()
                });
            }
        }
    }
}
