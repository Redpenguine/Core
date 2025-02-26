namespace Redpenguin.Core.StateManagement
{
  public interface IStateFactory
  {
    T GetState<T>() where T : class, IExitableState;
  }
}