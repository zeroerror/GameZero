namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileState_LockOnEntity : GameProjectileStateBase
    {
        public GameProjectileStateModel_LockOnEntity model;
        public void SetModel(in GameProjectileStateModel_LockOnEntity model) => this.model = model;

        public GameEntityBase lockOnEntity;

        public override void Clear()
        {
            base.Clear();
            this.lockOnEntity = null;
        }
    }
}