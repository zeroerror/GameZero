namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateModel_Attach : GameProjectileStateModelBase
    {
        public GameActionTargeterArgs targeter { get; private set; }
        public void SetTargeter(GameActionTargeterArgs targeter) => this.targeter = targeter;

        public override void Clear()
        {
            base.Clear();
        }
    }
}