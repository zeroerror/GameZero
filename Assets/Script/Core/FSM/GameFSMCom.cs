using System;
using System.Collections.Generic;

namespace GamePlay.Core
{
    public class GameFSMCom
    {
        private Dictionary<string, GameStateBase> _stateDict = new();
        private GameStateBase _curState;
        private List<GameStateTransition> _transitions = new();
        private List<GameStateTransition> _anyTransitions = new();

        public GameFSMCom() { }

        public void CreateState(GameStateBase state)
        {
            if (_stateDict.ContainsKey(state.stateName))
                return;

            _stateDict[state.stateName] = state;
        }

        public void SetTransition(GameStateTransition transition)
        {
            SetTransition(transition, _transitions);
        }

        public void SetAnyTransition(GameStateTransition transition)
        {
            SetTransition(transition, _anyTransitions);
        }

        private void SetTransition(GameStateTransition transition, List<GameStateTransition> byList)
        {
            var fromState = transition.fromState;
            var toState = transition.toState;
            if (!_stateDict.ContainsKey(fromState) || !_stateDict.ContainsKey(toState))
                return;

            var t = FindTransition(fromState, toState, byList);
            if (t == null) byList.Add(transition);
        }

        public void Tick(float dt)
        {
            this.TickAny(dt);
            if (_curState == null && _transitions.Count > 0) this.TransitTo(_transitions[0].toState);
            _curState?.Tick(dt);

            foreach (var transition in _transitions)
            {
                if (TryTransit(transition))
                    return;
            }
        }

        private void TickAny(float dt)
        {
            foreach (var transition in _anyTransitions)
            {
                if (TryTransit(transition))
                    return;
            }
        }

        private bool TryTransit(GameStateTransition transition)
        {
            if (transition.Condition())
            {
                TransitTo(transition.toState);
                return true;
            }
            return false;
        }

        public void TransitTo(string stateName)
        {
            _curState?.Exit();
            if (_stateDict.ContainsKey(stateName))
            {
                _curState = _stateDict[stateName];
                _curState?.Enter();
            }
        }

        private GameStateTransition FindTransition(string fromState, string toState, List<GameStateTransition> byList)
        {
            return byList.Find(item => item.fromState == fromState && item.toState == toState);
        }
    }
}
