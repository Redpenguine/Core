using Cysharp.Threading.Tasks;

namespace Redpenguin.Core.StateManagement
{
  public interface IState: IExitableState
  {
    void Enter();
  }
  public interface IStateAsync: IState
  {
    UniTask EnterAsync();
  }
}