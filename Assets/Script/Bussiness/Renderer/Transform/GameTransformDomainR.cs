using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameTransformDomainR : GameTransformDomainApiR
    {
        GameContextR _context;

        public GameTransformDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            this._BindEvent();
        }

        public void Destroy()
        {
            this._UnbindEvents();
        }

        private void _BindEvent()
        {
            this._context.BindRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC, this._OnTransformSync);
        }
        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC, this._OnTransformSync);
        }

        private void _OnTransformSync(object args)
        {
            var evArgs = (GameTransformRCArgs_Sync)args;
            ref var idArgs = ref evArgs.idArgs;
            GameEntityBase entity = this._context.FindEntity(idArgs);
            if (entity == null)
            {
                this._context.DelayRC(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC, args);
                return;
            }
            entity.transformCom.SetByArgs(evArgs.transArgs);
        }
    }
}
