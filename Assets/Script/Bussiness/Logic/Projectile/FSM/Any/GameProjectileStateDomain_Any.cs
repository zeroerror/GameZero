using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_Any : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity projectile)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity projectile)
        {
        }

        protected override void _Tick(GameProjectileEntity projectile, float dt)
        {
            var stateModel = projectile.fsmCom.anyState;
            stateModel.stateTime += dt;
            this._TickKeyFrame(projectile, dt);
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity projectile)
        {
            var lifeTime = projectile.model.lifeTime;
            if (lifeTime > 0 && projectile.fsmCom.anyState.stateTime >= lifeTime) return GameProjectileStateType.Destroyed;
            return base._CheckExit(projectile);
        }

        private void _TickKeyFrame(GameProjectileEntity projectile, float dt)
        {
        }
    }

}