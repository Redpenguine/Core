using Cysharp.Threading.Tasks;

namespace Redpenguin.Core.StateManagement
{
    public abstract class SimpleState : IState
    {
        public virtual void Enter()
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