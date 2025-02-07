using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileStateDomain_IdleR : GameProjectileStateDomainBaseR
    {
        public override void BindEvents()
        {
            base.BindEvents();
            this._context.BindRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_IDLE, this._OnEnter);
        }

        public override void UnbindEvents()
        {
            base.UnbindEvents();
            this._context.UnbindRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_IDLE, this._OnEnter);
        }

        private void _OnEnter(object args)
        {
            var rcArgs = (GameProjectileRCArgs_StateEnterIdle)args;
            GameLogger.Log("投射物状态进入 - 待机");
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