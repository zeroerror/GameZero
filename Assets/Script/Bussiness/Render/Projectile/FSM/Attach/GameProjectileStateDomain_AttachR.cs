using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;
namespace GamePlay.Bussiness.Render
{
    public class GameProjectileStateDomain_AttachR : GameProjectileStateDomainBaseR
    {

        public override void BindEvents()
        {
            base.BindEvents();
            this._context.BindRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_ATTACH, this._OnEnter);
        }

        public override void UnbindEvents()
        {
            base.UnbindEvents();
            this._context.UnbindRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_ATTACH, this._OnEnter);
        }

        private void _OnEnter(object args)
        {
            var rcArgs = (GameProjectileRCArgs_StateEnterAttach)args;
            var projectile = this._context.FindEntity(rcArgs.idArgs) as GameProjectileEntityR;
            if (projectile == null)
            {
                this._context.DelayRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_ATTACH, rcArgs);
                return;
            }

            projectile.fsmCom.EnterAttach();
            projectile.fsmCom.attachState.attachEntity = this._context.FindEntity(rcArgs.targetIdArgs);
            GameLogger.Log("投射物状态进入 - 附着");
        }

        public override void Enter(GameProjectileEntityR projectile)
        {
        }

        protected override void _Tick(GameProjectileEntityR projectile, float frameTime)
        {
            var attachEntity = projectile.fsmCom.attachState.attachEntity;
            if (attachEntity is GameRoleEntityR role)
            {
                role.bodyCom.tmRoot.TryGetSortingOrder(out var order, out var layer);
                projectile.root.SetSortingOrder(order + 1, layer);
            }
        }
    }

}