using System;

namespace Redpenguin.StateManagement.Default
{
    public interface IStateMachine : IDisposable
    {
        IStateBase CurrentState { get; }
        void AddState<TState>(TState state) where TState : class, IStateBase;
        void Enter<TState>() where TState : class, IState;
        void Enter(Type type);
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
        void AddState(Type type, IStateBase stateInstance);
    }
}