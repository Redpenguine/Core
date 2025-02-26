using Cysharp.Threading.Tasks;

namespace Redpenguin.Core.StateManagement
{
    public abstract class SimplePayloadStateAsync<TPayload> : IPayloadStateAsync<TPayload>
    {
        void IPayloadState<TPayload>.Enter(TPayload payload)
        {
        }

        protected virtual void Exit()
        {
        }

        public virtual UniTask EnterAsync(TPayload payload)
        {
            return UniTask.CompletedTask;
        }

        UniTask IExitableState.BeginExit()
        {
            Exit();
            return UniTask.CompletedTask;
        }

        void IExitableState.EndExit()
        {
        }
    }
}