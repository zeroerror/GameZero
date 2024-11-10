namespace Game.Core
{
    public abstract class GameStateTransition
    {
        public abstract string fromState { get; set; }
        public abstract string toState { get; set; }
        public abstract bool Condition();
    }
}
