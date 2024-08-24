using System;
using System.Collections.Generic;

namespace Redpenguin.StateManagement.TransitionsSeparately
{
    public class StateMachine : IStateMachine
    {
        public IState CurrentState { get; private set; }

        private readonly Dictionary<Type, List<Transition>> _transitions = new();
        private List<Transition> _currentTransitions = new();
        private readonly List<Transition> _anyTransitions = new();

        private static readonly List<Transition> EmptyTransitions = new(0);

        public void Execute()
        {
            var transition = GetTransition();
            while (transition != null)
            {
                SetState(transition.To); 
                transition = GetTransition();
            }

            CurrentState?.OnStay();
        }

        public void SetState(IState state)
        {
            if (state == CurrentState)
                return;

            CurrentState?.OnExit();
            CurrentState = state;

            _transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions);
            _currentTransitions ??= EmptyTransitions;

            CurrentState.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
        }

        private class Transition
        {
            public Func<bool> Condition { get; }
            public IState To { get; }

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Condition())
                    return transition;

            foreach (var transition in _currentTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }
    }
}