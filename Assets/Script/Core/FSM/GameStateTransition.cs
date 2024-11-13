using System;
namespace GamePlay.Core
{
    public abstract class GameStateTransition
    {
        public abstract string fromState { get; }
        public abstract string toState { get; }
        public abstract bool Condition();
    }
}
