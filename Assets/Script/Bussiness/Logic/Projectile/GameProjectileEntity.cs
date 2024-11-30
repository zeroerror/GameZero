namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileEntity : GameEntityBase
    {
        public GameProjectileFSMCom fsmCom { get; private set; }

        public GameProjectileEntity(GameProjectileModel model) : base(model.typeId, GameEntityType.Projectile)
        {
            this.fsmCom = new GameProjectileFSMCom();
        }
    }
}