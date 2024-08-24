using System;
using System.Collections.Generic;
using UnityEngine;

namespace Redpenguin.StateManagement.EnumBased
{
    public class StateMachine<T> : IStateMachine<T> where T : Enum
    {
        private readonly Dictionary<T, IState<T>> _states = new();

        private IState<T> _currentState;
        public IState<T> CurrentState => _currentState;

        public void AddState(IState<T> state)
        {
            _states.TryAdd(state.StateType, state);
        }

        public void Enter(T state)
        {
            if (!_states.TryGetValue(state, out var newState))
            {
                Debug.LogError($"State {state} not found");
                return;
            }
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
    
    public interface IStateMachine<T> where T : Enum
    {
        IState<T> CurrentState { get; }
        void AddState(IState<T> state);
        void Enter(T state);
    }
    
    public interface IState<out T> : IState where T : Enum
    {
        T StateType { get; }
    }

    public interface IState
    {
        void Enter();
        void Exit();
    }
}