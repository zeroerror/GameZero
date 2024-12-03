using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileStateDomain_DestroyedR : GameProjectileStateDomainBaseR
    {
        public override void BindEvents()
        {
            base.BindEvents();
            this._context.BindRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_DESTROYED, this._OnEnter);
        }

        public override void UnbindEvents()
        {
            base.UnbindEvents();
            this._context.UnbindRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_DESTROYED, this._OnEnter);
        }

        private void _OnEnter(object args)
        {
            var evArgs = (GameProjectileRCArgs_StateEnterDestroyed)args;
            var projectile = this._projectileContext.repo.FindByEntityId(evArgs.idArgs.entityId);
            if (projectile == null)
            {
                this._context.DelayRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_DESTROYED, args);
                return;
            }
            projectile.SetValid(false);
            GameLogger.Log("投射物状态进入 - 销毁");
        }

        public override void Enter(GameProjectileEntityR entity)
        {
            throw new System.NotImplementedException();
        }

        protected override void _Tick(GameProjectileEntityR entity, float frameTime)
        {
            throw new System.NotImplementedException();
        }
    }

}