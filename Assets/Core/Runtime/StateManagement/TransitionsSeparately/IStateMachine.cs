using System;

namespace Redpenguin.StateManagement.TransitionsSeparately
{
    public interface IStateMachine
    {
        IState CurrentState { get; }
        void Execute();
        void SetState(IState state);
        void AddTransition(IState from, IState to, Func<bool> predicate);
        void AddAnyTransition(IState state, Func<bool> predicate);
    }
}