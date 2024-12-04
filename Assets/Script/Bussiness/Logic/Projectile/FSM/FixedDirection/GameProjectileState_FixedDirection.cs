namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileState_FixedDirection : GameProjectileStateBase
    {
        private GameProjectileStateModel_FixedDirection model;
        public void SetModel(in GameProjectileStateModel_FixedDirection model) => this.model = model;

        public override void Clear()
        {
            base.Clear();
        }
    }
}