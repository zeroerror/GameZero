using System;
namespace GamePlay.Core
{
    public abstract class GameStateTransition<T>
    {
        public GameStateBase<T> fromState { get; private set; }
        public GameStateBase<T> toState { get; private set; }
        public Action<T> onTransition { get; private set; }
        public abstract bool TickCondition(float dt, T obj);

        public GameStateTransition(GameStateBase<T> fromState, GameStateBase<T> toState, Action<T> onTransition = null)
        {
            this.fromState = fromState;
            this.toState = toState;
            this.onTransition = onTransition;
        }
    }

    internal class GameDefaultStateTransition<T> : GameStateTransition<T>
    {
        public GameDefaultStateTransition(GameStateBase<T> toState) : base(null, toState) { }
        public override bool TickCondition(float dt, T obj) => true;
    }
}
