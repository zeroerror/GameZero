namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileState_FixedDirection : GameProjectileStateBase
    {
        public GameProjectileStateModel_FixedDirection model { get; private set; }
        public void SetModel(in GameProjectileStateModel_FixedDirection model) => this.model = model;

        public override void Clear()
        {
            base.Clear();
        }
    }
}