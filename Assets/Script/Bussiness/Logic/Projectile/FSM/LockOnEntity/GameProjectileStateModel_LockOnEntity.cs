namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateModel_LockOnEntity : GameProjectileStateModelBase
    {
        public GameEntityBase lockOnEntity { get; private set; }

        public void SetLockOnEntity(GameEntityBase entity)
        {
            this.lockOnEntity = entity;
        }

        public override void Clear()
        {
            base.Clear();
            lockOnEntity = null;
        }
    }
}