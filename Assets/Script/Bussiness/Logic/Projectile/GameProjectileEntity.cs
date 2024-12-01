using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileEntity : GameEntityBase
    {
        public GameProjectileModel model { get; private set; }
        public GameProjectileFSMCom fsmCom { get; private set; }

        public GameProjectileEntity(GameProjectileModel model) : base(model.typeId, GameEntityType.Projectile)
        {
            this.model = model;
            this.fsmCom = new GameProjectileFSMCom();
        }

        public override void Tick(float dt)
        {
        }

        public override void Dispose()
        {
        }

        public void FaceTo(in GameVec2 dir)
        {
            this.transformCom.forward = dir;
            this.transformCom.angle = GameVec2.SignedAngle(GameVec2.right, dir);
        }
    }
}