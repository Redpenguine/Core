using Cysharp.Threading.Tasks;

namespace Redpenguin.Core.StateManagement
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;
        UniTask EnterAsync<TState>() where TState : class, IState;
        UniTask EnterAsync<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;
    }
}