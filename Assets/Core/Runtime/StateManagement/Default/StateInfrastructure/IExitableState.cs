
using Cysharp.Threading.Tasks;

namespace Redpenguin.Core.StateManagement
{
  public interface IExitableState
  {
    UniTask BeginExit();
    void EndExit();
  }
}