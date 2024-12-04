namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileState_Attach : GameProjectileStateBase
    {
        public GameProjectileStateModel_Attach model;
        public void SetModel(in GameProjectileStateModel_Attach model) => this.model = model;

        public GameActionTargeterArgs targeter { get; private set; }
        public void SetTargeter(GameActionTargeterArgs targeter) => this.targeter = targeter;

        public override void Clear()
        {
            base.Clear();
        }
    }
}