using Cysharp.Threading.Tasks;

namespace Redpenguin.Core.StateManagement
{
    public abstract class SimplePayloadState<TPayload> : IPayloadState<TPayload>
    {
        public virtual void Enter(TPayload payload)
        {
        }

        protected virtual void Exit()
        {
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