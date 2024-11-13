namespace GamePlay.Bussiness.Logic
{
    public abstract class GameEntityContextBase
    {
        public abstract GameEntityRepoBase repo { get; }
        public GameEntityContextBase() { }
    }
}