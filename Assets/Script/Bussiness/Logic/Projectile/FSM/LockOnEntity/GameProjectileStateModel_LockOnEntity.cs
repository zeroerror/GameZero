namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateModel_LockOnEntity : GameProjectileStateModelBase
    {
        public GameEntityBase lockOnEntity { get; private set; }
        public override void Clear()
        {
            base.Clear();
            lockOnEntity = null;
        }
    }
}