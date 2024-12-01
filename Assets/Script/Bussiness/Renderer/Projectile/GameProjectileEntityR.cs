using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.Renderer
{
    public class GameProjectileEntityR : GameEntityBase
    {
        public readonly GameProjectileModel model;
        public GameProjectileFSMCom fsmCom { get; private set; }
        public GamePlayableCom animCom { get; private set; }
        public GameTimelineCom timelineCom { get; private set; }

        public GameProjectileEntityR(GameProjectileModel model) : base(model.typeId, GameEntityType.Projectile)
        {
            this.fsmCom = new GameProjectileFSMCom();
            this.animCom = new GamePlayableCom();
            this.timelineCom = new GameTimelineCom();
            this.model = model;
        }

        public void PlayAnim()
        {
            this.animCom.Play(this.model.animClip);
        }
    }
}