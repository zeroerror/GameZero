namespace GamePlay.Core
{
    public abstract class GameStateBase<T>
    {
        public abstract string stateName { get; }

        public override int GetHashCode()
        {
            return stateName.GetHashCode();
        }

        public override string ToString()
        {
            return stateName;
        }

        public abstract void Enter(T obj);
        public abstract void Tick(float dt, T obj);
        public abstract void Exit(GameStateBase<T> nextState, T obj);
        public virtual void Dispose() { }
    }
}
