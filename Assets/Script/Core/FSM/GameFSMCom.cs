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
            if (_curState == null)
                TransitTo(state.stateName);
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

            var existingTransition = FindTransition(fromState, toState, transition.Condition, byList);
            if (existingTransition != null)
            {
                existingTransition.Condition = transition.Condition;
            }
        }

        public void Tick(float dt)
        {
            TickAny(dt);
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

        private GameStateTransition FindTransition(string fromState, string toState, Func<bool> condition, List<GameStateTransition> byList)
        {
            return byList.Find(item => item.fromState == fromState && item.toState == toState && item.Condition == condition);
        }
    }
}
