using Cysharp.Threading.Tasks;

namespace Redpenguin.Core.StateManagement
{
    public interface IPayloadState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }

    public interface IPayloadStateAsync<TPayload> : IPayloadState<TPayload>
    {
        UniTask EnterAsync(TPayload payload);
    }
}