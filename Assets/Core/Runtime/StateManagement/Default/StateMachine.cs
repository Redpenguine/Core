using System;
using System.Collections.Generic;

namespace Redpenguin.StateManagement.Default
{
    public class StateMachine : IStateMachine
    {
        public IStateBase CurrentState => ActiveState;
        protected readonly Dictionary<Type, IStateBase> States = new();
        protected IStateBase ActiveState;

        public void AddState<TState>(TState state) where TState : class, IStateBase
        {
            AddState(typeof(TState), state);
        }
        public void AddState(Type type, IStateBase stateInstance)
        {
            States.TryAdd(type, stateInstance);
        }

        public void Enter<TState>() where TState : class, IState
        {
            Enter(typeof(TState));
        }

        public void Enter(Type type)
        {
            var state = ChangeState(type);
            ((IState) state).Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState(typeof(TState));
            ((TState)state).Enter(payload);
        }

        protected IStateBase ChangeState(Type type)
        {
            ActiveState?.Exit();
            var state = GetState(type);
            ActiveState = state;
            return state;
        }

        protected IStateBase GetState(Type type) 
        {
            return States[type];
        }

        public void Dispose()
        {
            foreach (var statesValue in States.Values)
            {
                if(statesValue is IDisposable disposable)
                    disposable.Dispose();
            }
            States.Clear();
        }
    }
}