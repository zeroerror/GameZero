namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileState_FixedDirection : GameProjectileStateBase
    {
        public GameProjectileStateModel_FixedDirection model { get; private set; }
        public void SetModel(in GameProjectileStateModel_FixedDirection model) => this.model = model;

        public int bounceCount { get; private set; }

        public override void Clear()
        {
            base.Clear();
        }
    }
}