using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Renderer
{
    public class GameAttributeDomainR : GameAttributeDomainApiR
    {
        GameContextR _context;

        public GameAttributeDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            this._BindEvents();
        }

        public void Destroy()
        {
            this._UnbindEvents();
        }

        private void _BindEvents()
        {
            this._context.BindRC(GameAttributeRCCollection.RC_GAME_ATTRIBUTE_SYNC, this._OnAttributeSync);
            this._context.BindRC(GameAttributeRCCollection.RC_GAME_ATTRIBUTE_BASE_SYNC, this._OnAttributeBaseSync);
        }
        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameAttributeRCCollection.RC_GAME_ATTRIBUTE_SYNC, this._OnAttributeSync);
            this._context.UnbindRC(GameAttributeRCCollection.RC_GAME_ATTRIBUTE_BASE_SYNC, this._OnAttributeBaseSync);
        }

        private void _OnAttributeSync(object args)
        {
            var rcArgs = (GameAttributeRCArgs_Sync)args;
            ref var idArgs = ref rcArgs.idArgs;
            GameEntityBase entity = this._context.FindEntity(idArgs);
            if (entity == null)
            {
                this._context.DelayRC(GameAttributeRCCollection.RC_GAME_ATTRIBUTE_SYNC, args);
                return;
            }
            entity.attributeCom.SetByArgs(rcArgs.attrAgrs);
        }

        private void _OnAttributeBaseSync(object args)
        {
            var rcArgs = (GameAttributeRCArgs_BaseSync)args;
            ref var idArgs = ref rcArgs.idArgs;
            GameEntityBase entity = this._context.FindEntity(idArgs);
            if (entity == null)
            {
                this._context.DelayRC(GameAttributeRCCollection.RC_GAME_ATTRIBUTE_BASE_SYNC, args);
                return;
            }
            entity.baseAttributeCom.SetByArgs(rcArgs.attrAgrs);
        }
    }
}
