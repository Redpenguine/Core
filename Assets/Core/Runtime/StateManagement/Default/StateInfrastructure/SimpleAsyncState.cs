using Cysharp.Threading.Tasks;

namespace Redpenguin.Core.StateManagement
{
    public abstract class SimpleAsyncState : IStateAsync
    {
        public virtual UniTask EnterAsync()
        {
            return UniTask.CompletedTask;
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

        void IState.Enter()
        {
            throw new System.NotImplementedException();
        }
    }
}