namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileEntity : GameEntityBase
    {
        public GameProjectileModel model { get; private set; }
        public GameProjectileFSMCom fsmCom { get; private set; }
        public GameProjectileStateTriggerCom triggerCom { get; private set; }

        public GameProjectileEntity(GameProjectileModel model) : base(model.typeId, GameEntityType.Projectile)
        {
            this.model = model;
            this.fsmCom = new GameProjectileFSMCom();
            this.triggerCom = new GameProjectileStateTriggerCom();
        }
    }
}