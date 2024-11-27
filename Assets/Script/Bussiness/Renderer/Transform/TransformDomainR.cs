using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class TransformDomainR : GameTransformDomainApiR
    {
        GameContextR _context;

        public TransformDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            this._BindEvent();
        }

        public void Dispose()
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
            GameEntityBase entity = null;
            switch (idArgs.entityType)
            {
                case GameEntityType.Role:
                    entity = this._context.roleContext.repo.FindByEntityId(idArgs.entityId);
                    break;
                default:
                    GameLogger.LogError("TransformDomainR._OnTransformSync: unknown entityType: " + idArgs.entityType);
                    break;
            }
            if (entity == null)
            {
                this._context.delayRCEventService.Submit(GameTransformRCCollection.RC_GAME_TRANSFORMN_SYNC, args);
                return;
            }
            entity.transformCom.SetByArgs(evArgs.transComArgs);
        }
    }
}
