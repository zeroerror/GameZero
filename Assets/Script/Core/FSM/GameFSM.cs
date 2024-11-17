using System.Collections.Generic;

namespace GamePlay.Core
{
    public class GameFSM<T>
    {
        private Dictionary<GameStateBase<T>, GameStateBase<T>> _stateDict;
        public GameStateBase<T> curState { get; private set; }
        public GameStateBase<T> lastState { get; private set; }
        public GameStateTransition<T> defaultTransition { get; private set; }

        protected List<GameStateTransition<T>> _transitions;
        private List<GameStateTransition<T>> _anyTransitions;

        public GameFSM()
        {
            _stateDict = new Dictionary<GameStateBase<T>, GameStateBase<T>>();
            _transitions = new List<GameStateTransition<T>>();
            _anyTransitions = new List<GameStateTransition<T>>();
        }

        public virtual void Dispose()
        {
            foreach (var state in _stateDict.Values)
            {
                state.Dispose();
            }
            _stateDict.Clear();
            _transitions.Clear();
            _anyTransitions.Clear();
        }

        public void AddState(GameStateBase<T> state)
        {
            if (_stateDict.ContainsKey(state)) return;
            _stateDict[state] = state;
            if (this.curState == null && this.defaultTransition == null) this.SetDefaultState(state);
        }

        public void SetDefaultState(GameStateBase<T> state)
        {
            if (state == null) return;
            if (_stateDict.ContainsKey(state))
            {
                this.defaultTransition = new GameDefaultStateTransition<T>(state);
            }
        }

        public void SetTransition(GameStateTransition<T> transition)
        {
            _SetTransition(transition, _transitions);
        }

        public void SetAnyTransition(GameStateTransition<T> transition)
        {
            _SetTransition(transition, _anyTransitions);
        }

        private void _SetTransition(GameStateTransition<T> transition, List<GameStateTransition<T>> byList)
        {
            var fromState = transition.fromState;
            var toState = transition.toState;
            if ((fromState != null && !_stateDict.ContainsKey(fromState)) || !_stateDict.ContainsKey(toState))
                return;

            var t = FindTransition(fromState, toState, byList);
            if (t == null) byList.Add(transition);
        }

        public void Tick(float dt, T obj)
        {
            if (this.curState == null) this.TransitTo(this.defaultTransition, obj);
            this.TickAny(dt, obj);
            curState?.Tick(dt, obj);

            foreach (var transition in _transitions)
            {
                if (TickTransition(dt, transition, obj))
                    return;
            }
        }

        private void TickAny(float dt, T obj)
        {
            foreach (var transition in _anyTransitions)
            {
                if (TickTransition(dt, transition, obj)) return;
            }
        }

        private bool TickTransition(float dt, GameStateTransition<T> transition, T obj)
        {
            var fromState = transition.fromState;
            if (fromState != this.curState) return false;
            if (transition.TickCondition(dt, obj))
            {
                TransitTo(transition, obj);
                return true;
            }
            return false;
        }

        public void TransitTo(GameStateTransition<T> transition, T obj)
        {
            if (transition == null) return;
            var toState = transition.toState;
            if (!this._stateDict.ContainsKey(toState)) return;
            this.curState?.Exit(toState, obj);
            this.lastState = curState;
            this.curState = toState;
            this.curState?.Enter(obj);
            transition.onTransition?.Invoke(obj);
        }



        private GameStateTransition<T> FindTransition(GameStateBase<T> fromState, GameStateBase<T> toState, List<GameStateTransition<T>> byList)
        {
            return byList.Find(item => item.fromState == fromState && item.toState == toState);
        }
    }
}
