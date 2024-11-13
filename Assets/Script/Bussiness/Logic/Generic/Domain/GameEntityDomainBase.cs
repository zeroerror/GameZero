namespace GamePlay.Bussiness.Logic
{
    public abstract class GameEntityDomainBase<T> where T : GameEntity
    {
        public abstract GameEntityContextBase context { get; }

        public GameEntityDomainBase() { }

        public abstract void Tick(float dt);
        public abstract T Create();
        public abstract void Collect();
    }
}